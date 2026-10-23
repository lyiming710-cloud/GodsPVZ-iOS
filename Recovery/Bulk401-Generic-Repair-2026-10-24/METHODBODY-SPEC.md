# Native-backed changes and qualification limits

Eleven direct compiler targets plus one proven damaged dependency, all rebuilt directly from immutable Native93. All inherited 389 raw method bodies and all 1916 non-target methods remain unchanged.

| Method | Exact token | Original native | Restored behavior |
|---|---|---|---|
| Map.RandomGet_Grid_TestPlace<T>(num,test) | 0600028F | 180490A10 | Calls 06000290<T>(num,test,0,GetMapX()-1); RGCTX entry 1 independently resolves the overload. |
| SwfAssocList<T>.get_Item(int) | 060008D4 | 1809D46E0 | Reads _list, forwards integer index to SwfList<T>.get_Item. |
| SwfAssocList<T>.get_Item(T) | 060008D5 | 1809D44F0 | Reads _dict, returns Dictionary<T,int>.get_Item(key). |
| SwfAssocList<T>.get_Count | 060008D6 | 18065BAA0 | Returns contained list count, preserves null exception. |
| SwfAssocList<T>.Contains | 060008D7 | 1809D3730 | Calls Dictionary<T,int>.ContainsKey. |
| SwfAssocList<T>.Add | 060008D8 | 1809D3110 | If absent, add key/current count to dictionary, reload _list after callbacks, then Push. Generic arguments retain !0/!1. |
| SwfList<T>.Push (dependency) | 060008E5 | 1809D5020 | If full, allocate capacity 4 or 2*size and copy; size++ before store; preserves failure/mutation order. |
| SwfList<T>.Pop | 060008E6 | 1809D4D50 | Empty throws InvalidOperationException("empty list"); decrement before data failure; save previous item, clear slot to default(T), return saved item. |
| SwfList<T>.Peek | 060008E7 | 1809D4CC0 | Empty exception, otherwise data[size-1], no size mutation. |
| SwfList<T>.get_Item | 060008EA | 1809D5990 | Unsigned index < size guard before data access; otherwise IndexOutOfRangeException. |
| SwfList<T>.get_Capacity | 060008ED | 18065BAA0 | Returns data.Length; throws on null data. |
| SwfUtils.GetComponent<T> | 060008F8 | 1804B2F00 | GetComponent; if addIfMissing and Unity op_Implicit false then AddComponent; exact identity and callback order. |

Original registration, RGCTX, field type and method alias artifacts are in BULK401-SOURCE.zip/evidence. Full/shared instances were inspected, and split .pdata regions were not mistaken for complete function extents. Original machine code has not yet been dynamically compared for these twelve methods; native evidence here is static control/dataflow.

Runtime tests load and execute the ten actual closed collection methods from the candidate. The Map and Unity wrappers are transcribed instruction for instruction from candidate CIL with explicit controlled external calls. This does not execute the real Unity engine or qualify the called Map overload. Reference/int/24-byte struct values, null/bounds/empty paths, state changes before exceptions, array growth/copy, duplicate keys, and comparer callback field replacement are covered.

Seven type-correct semantic mutants are rejected by actual execution: wrong Pop result, signed bounds, constant dictionary index, stale list after callback, omitted array copy, wrong Map lower bound, inverted Unity truthiness. The old R4 also fails an actual runtime assertion; the first control expected InvalidProgramException, but actual execution returned the wrong Pop value. The first report is retained; the corrected control expects the observed semantic failure.

The frozen checker is not edited. Closed-context static evidence substitutes real concrete arguments and models box of a known reference parameter as identity, retaining offsets. Its 412 passing contexts and five rejected controls are finite evidence, not universal generic verification. The initial unsupported generic receiver/boxing results remain preserved.

Linux fresh IL2CPP conversion completed in 20.29 s; all 756 CPP and 274 C compiled without errors. The damaged R4 control separately regenerated and produced 37 expected diagnostics in its selected two generic units; this selected control is not claimed as a fresh full R4 census. Every one of the new 1030 source hashes and raw logs was independently read back.

Apple arm64 syntax checking uses the real Xcode 16.4 / iPhoneOS 18.5 compiler flags and runtime headers from the preserved Unity export, with the fresh portable-profile generated code. It still cannot replace a real new Unity export, final native/runtime/Objective-C compilation, linking, unsigned IPA inspection or device acceptance. No IPA is currently claimed.
