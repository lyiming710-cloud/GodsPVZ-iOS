# Native17: SwfList<T>.AssignTo(List<T>)

Single MethodBody replacement, MethodDef **0x060008EF**, global definition
44052. The SwfList overload 0x060008F0 and managed get_Item remain unchanged.
Native reference body is 0x1809D4710..0x1809D4890 (384 bytes), spec 51859,
pointer 45923; fully shared body is 0x1809D4980..0x1809D4B75 (501 bytes),
spec 51874, pointer 45938. Exact body hashes and class instantiations are in
mapping.json. The PC input identities are the same locked GameAssembly and
metadata documented in Native16; extraction uses the same registration tables.

The native method clears the destination List<T>, including version increment
and clearing old references. If destination.Capacity < this._size, set capacity
to unchecked(this._size*2). Capture this._size as the loop limit. For each index,
perform the native inlined unsigned index-vs-current-size check, load _data[index],
and call destination.Add. The damaged managed indexer is not called. This keeps
the write boundary at one method while restoring the observed native body.
There is no AddRange replacement, source clearing, defensive null return, or
rollback after partial copying. List<T> Clear/Capacity/Add use the pinned Unity
framework signatures; the CLR fixture uses the host framework only in fixture mode.

Nineteen CLR assertions passed: ordered reference/value/struct copies, null
elements, exact capacity policy, logical-size limit, destination clearing and
enumerator invalidation, empty/negative counts, unchecked capacity overflow,
null input, partial copy on short backing array, and unchanged sibling overload.
The emitted 123-byte body has 39 instructions, two locals and no EH. One
deliberately damaged-stack control is rejected. Production metadata and all
non-target bodies are checked before writing and after reopening.

Reproduce after native14 bootstrap:

```sh
python3 scripts/codespaces/test_native17_fixture.py
python3 scripts/codespaces/validate_native17.py
```

The replay checks native16 hashes and its exact AssignTo failure as the positive
control, then materializes both candidates twice and invokes real IL2CPP.
At source `659a403a57fd544c24b7c2073aabc55f09a4ea6c`, the complete Codespace
replay finished in **219.57 seconds**: **NATIVE17_DIRECT_CONVERSION_PASS**,
IL2CPP **exit 0**, methods **[]**. Native16 was independently reproduced with
the exact preceding failure. All 2296 non-target MethodBodies were unchanged;
linked/unlinked output pairs were byte-identical.

- Unlinked SHA256: `74c7474f0592d3f577f8376d75d26528624798794983b3f07db5fd973e7456b2`
- Linked SHA256: `9bf2d7a033632061e8e71d32c70312c3c234298649495afc384d0dfe7120a125`

Raw logs and hash manifest are in validation/. A fresh full Unity import/export
is the next independent acceptance stage; direct post-Linker conversion success
does not claim that stage or device-runtime acceptance.
