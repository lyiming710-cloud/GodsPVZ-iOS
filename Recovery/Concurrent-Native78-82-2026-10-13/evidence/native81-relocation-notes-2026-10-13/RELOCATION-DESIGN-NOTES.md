# Native81 Project.GetEndPosition relocation notes

Static source inspection only. No candidate, patcher, tests, target assembly, compiler, runtime, or acceptance execution was performed. Existing files were read only; this note is the only output of this subtask.

## Existing exact source patterns

There are already raw byte patchers for this operation; no new broad design is necessary:

| Source | Relevant lines | Source SHA-256 |
| --- | --- | --- |
| `audit/native30-continuation-2026-10-02/Patcher40.cs` | 25–29 | `4303b7171e9aec1e2f909d39301ca075f6af8368985a2b65b6d2303c0a058e9a` |
| `audit/native30-continuation-2026-10-02/Patcher43.cs` | 12–16 | `744c8c3df9e6c82330bd7b8e46e7665da3d66cea4b6d5ad5d1db032f8deb1131` |
| `audit/native30-continuation-2026-10-02/Patcher50.cs` | 18–21 | `f10cc69b83a4cdb5fe1088c045635430c2d5bbf218cc0d6bd0e53ef0751fd801` |
| Current template `audit/native79-codex-2026-10-13/Patcher79.cs` | `Metadata()` plus final target-write loop | `d43324bdcf4e26a80616d7c3b8b66d66c6eb21e43ce26669af8fc92b57d3acc2` |

Patcher40 and Patcher43 append one complete 12-byte-header fat body to existing `.n29meta`, copy the metadata root after the body, and update exactly the target MethodDef row's first four bytes in that copied root. Their original method slots remain intact. Patcher50 does this for two methods and explicitly 4-aligns each body start. These are source design references; this subtask did not replay them or promote prior qualification claims.

The precise row-address formula used in all three is:

```csharp
int rowInRoot = md.GetTableMetadataOffset(TableIndex.MethodDef)
    + (checked((int)method.MetadataToken.RID) - 1)
      * md.GetTableRowSize(TableIndex.MethodDef);
BitConverter.GetBytes(newRva).CopyTo(metadataRootCopy, rowInRoot);
```

`GetTableMetadataOffset` here is relative to the metadata root, as the source's direct write into the root byte slice shows. It is not a PE file offset and should not be combined with the `#~` stream offset a second time. Patcher79's own insertion code separately subtracts `tableStream` when addressing the standalone `#~` byte array, corroborating those coordinate systems.

MethodDef starts with `RVA:uint32`, followed by implementation flags, method flags, name, signature and ParamList indexes. Consequently only the first four bytes of the target row change. Compute the row size from the selected reader; do not assume 14 bytes even if the current narrow-index layout yields that size. `0x060003C5` has RID 965, so its row index is 964.

## Recommended placement after Patcher79.Metadata

The cleanest composition with the current template is **metadata first, then the Project body at final EOF**. The new body does not require moving the newly expanded active metadata a second time:

```text
parent image
    -> existing Metadata() expansion with approved appended rows/heaps
    -> current expanded image and its current active metadata root
    -> append aligned Project fat body at expanded-image EOF
    -> change current active Project MethodDef RVA field
    -> extend the same last section and PE sizes
```

Reparse the in-memory byte array returned by `Metadata()` with a fresh PEReader/MetadataReader before deriving the active-root offset and MethodDef row position. Do not reuse a parent PEReader's original root RVA/file offset, and do not hardcode a row offset calculated before adding metadata. Appending only tables numbered after MethodDef may leave the MethodDef table's location stable, but a future earlier-table addition, widened index, or changed stream placement can move it; a fresh reader removes that dependency.

The old Cecil MethodDefinition can still supply the stable target token, expected original RVA, unchanged InitLocals, and unchanged local signature. The new reader's target row must independently show the expected old RVA before replacing it. No assembly execution is implied by this metadata parse.

## Minimal helper recipe, not an implementation

Inputs: the current metadata-expanded image; exact target token `0x060003C5`; expected previous MethodDef RVA; finished IL bytes; explicitly chosen MaxStack; existing local token `0x110002F3`; and the target's original InitLocals value.

1. Parse the current image. Require PE32, unsigned layout, last section `.n29meta`, characteristics `0x40000040`, no trailing overlay (`last.PointerToRawData + last.SizeOfRawData == current.Length`), and no old last-section virtual tail beyond raw bytes (`VirtualSize <= SizeOfRawData`). Require that the active metadata root is fully backed by that last section and the current file.
2. Find the current Project MethodDef by RID. Bound-check `rowInRoot + 4 <= metadataSize`, compute `rowFileOffset = activeRootFileOffset + rowInRoot`, and require that those four bytes equal the expected previous RVA. Require target previous body is a supported managed IL method with no EH/additional data sections, since this Project reconstruction also has none.
3. `bodyOffset = Align(current.Length, 4)`; `bodyRva = last.VirtualAddress + bodyOffset - last.PointerToRawData`, using checked arithmetic. Require `bodyRva % 4 == 0`; the selected input's RVA/raw translation must preserve this alignment. Use the old section RVA/raw start unchanged.
4. Write a complete 12-byte fat header at bodyOffset: flags/size `0x3003 | (InitLocals ? 0x10 : 0)`; MaxStack as UInt16; CodeSize as exact IL byte count; LocalVarSigTok `0x110002F3`. MoreSects must be clear for this Project body. Preserve InitLocals deliberately rather than merely copying an unparsed header word containing unrelated flags.
5. Place IL immediately after the header. `usedEnd = bodyOffset + 12 + code.Length`; `finalLength = Align(usedEnd, FileAlignment)`; allocate zero-initialized output and copy the entire current image first. Preserve original Project body bytes and every old method slot. Trailing file-alignment bytes are zero padding, not part of CodeSize.
6. Overwrite only `rowFileOffset..rowFileOffset+4` with bodyRva in the **currently active expanded metadata root**. Keep the active CLI MetadataDirectory RVA and Size unchanged, because the body was appended after that root.
7. Extend only the same last-section size fields and necessary PE totals:

```text
newVirtualSize = usedEnd - last.PointerToRawData
newRawSize     = finalLength - last.PointerToRawData
deltaRaw       = finalLength - current.Length

last-section VirtualSize       <- newVirtualSize
last-section SizeOfRawData     <- newRawSize
PE SizeOfInitializedData       <- current SizeOfInitializedData + deltaRaw
PE SizeOfImage                 <- Align(last.VirtualAddress + newVirtualSize,
                                       SectionAlignment)
PE CheckSum                    <- 0
```

Use the post-Metadata intermediate PE's sizes as the baseline, so the preceding metadata expansion is not counted twice. Check every multiplication, addition, alignment, allocation length, and RVA conversion for overflow. Section count, names, raw/virtual starts, attributes, CLI flags, entrypoint and all unrelated directories remain unchanged.

The precise raw header-field offsets in the existing patchers are: last section header `+8` VirtualSize and `+16` SizeOfRawData; PE optional header `+8` SizeOfInitializedData, `+56` SizeOfImage, `+64` checksum. Derive the last section header from the parsed PE header, optional-header size and section count as the sources do; do not hardcode the generation report's section-header offset.

The section remains initialized read-only data, matching the existing relocation patterns. This holds managed IL bytes; the helper should not add IMAGE_SCN_MEM_EXECUTE or another section. The active metadata directory ends before the appended method; its Size must not be enlarged to include method bytes.

## Scope boundaries for integration

- In the final target loop, branch to the relocation helper **only for 0x060003C5**. Do not first Array.Clear/write the old Project slot. Other targets can keep their separately authorized in-place/EH handling.
- This helper is specifically a no-EH Project body append. Patcher40/43/50 are not a ready generic EH relocator. The distinct 1AC typed-enumerator finally requires correct section/header encoding and its own slot/extent accounting; do not accidentally inherit their no-EH assumption for it.
- If the body reuses the eight existing Project locals, preserve `0x110002F3`; no new Project standalone local row is necessary. New List-related metadata for 1AC is already handled by the preceding scoped metadata expansion.
- The legitimate existing-row delta relative to the metadata-expanded intermediate is one 4-byte Project RVA field. Other MethodDef row bytes and all other method RVAs must stay equal. The old active metadata copy is left in place; only the now-active copied root gets the target RVA change.
- Record the original/current parent digest, intermediate root RVA/offset/size, target row offset, old/new target RVA, body file offset, flags, MaxStack, local signature, IL/header/padding extents, and exact permitted PE-header changes in generation evidence. A static source design note is not evidence that these constraints passed on a generated image.

The saved Native79 generation report records parent79 digest `3b3bcfca51402e1eedf5d59fbe1bf9d923336730325550205f960bd8a8bceec6`, active root file offset 11,588,096 and size 402,400 after Native79. These are report-derived checkpoint details only; the actual parent79 PE was not independently loaded in this subtask. Derive new layout dynamically from the exact pinned input bytes instead of carrying those numbers into code.
