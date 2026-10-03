# Native54 — four native-backed callers, pending 100-method full gate

This is a pre-full-build checkpoint, not complete compiler qualification. Candidate `820e030be1a7f1f64ffcc8d73fa3c11b9daedfb95c0981f9f46f3f818d333c52` derives exactly from Native53 `8e40b7920c1f689b9b5c8460df06d3122adef94fe8c87bdebe6b56d894aa7b27`.

* 0x0600043D Zombie.GetAnimationFrameXSpeed: capture transform position and previous X; test the first deltaTime against floating zero; when nonzero, divide the captured X difference by a second deltaTime call. Preserve NaN, signed zero and callback capture order.
* 0x0600043E Zombie.GetArmor1Position: original six-way armor selection; cone/bucket/brick/IceCube1/bucket/LadderSaboteurs_helmet1, default String.Empty. Pass animationSprites and the selected existing literal to GetAnimationSpritePosition. Uses immutable existing signature 0x110001AA; no metadata blob is edited.
* 0x0600043F Zombie.GetArmor2Position: capture list, public get_Item(0), gameObject.transform, transform.position by value. Preserve null and bounds failures and callback rereads.
* 0x060005D9 BoardPreview.GetRandomPosition: first Random.Range(680,980), then enemy row read, 286-row*134, then Random.Range(-20,100)+captured Y; Z is initialized to zero. The null Enemy check follows the first random call. No added null fallback.

16,384 original PC caller observations match actual emitted candidate CIL, with zero positive failures and zero tool errors. Ten emitted wrong variants, four actual wrong DLLs and ten forged-report/scope corruptions are detected. Native24–53 historical fixtures, frozen typed verification (all four targets pass) and 2,293 unchanged non-target raw bodies were verified before this checkpoint. All metadata/reference/PE bytes outside the four bounded slots remain unchanged. Original field offsets, float constants, switch targets and five tagged native literals are independently decoded and pinned.

The original native callers execute unchanged under explicitly initialized metadata and recording Unity/time/random/list/sprite/exception dependencies. Actual CLR uses the pinned public Vector3 and public List get_Item. This tests caller logic and dependency order; it does not prove the original Unity helpers, engine/class initialization, exception helper implementation, full game, or iOS behavior. Native partial hidden return-buffer writes on preview exceptions are recorded but excluded from managed return-value comparison because a throwing managed call exposes no returned value.

The full conversion was started before the user requested 100-method batches. It ended nonzero without complete reports; workspace free space was independently observed at zero and file writes failed with ENOSPC. The empty converter tail does not establish its exact failure cause. No C++ clearance count is assigned to Native54 and no retry is scheduled before the 100-method gate. Last completed full build is Native53: 516 errored methods / 5,668 diagnostics / 24 failing units.

0x060003C5 Project.GetEndPosition is included in read-only discovery evidence; it is not a repair target or part of the 4/100 count. No licensed Unity/Xcode/IPA/device build was performed.
