# Native111 source behavior requirements

These six sources are authored without primary project execution, construction or tests. The fixed construction parent remains Native93; Bulk162 R1 is the previous source aggregate. Source counts do not represent compiler or behavior acceptance.

## Plant.Update_Color (0352)

Start with RGB factors (1,1,1). Evaluate board.mouseManager.plantOnGlove == this using Unity equality; the true branch changes all three factors to the exact float32 value 0.4. Call the current elementManager.GetElementColor and multiply each RGB component by its factor. Enumerate the current private animationSprites List<GameObject> using its public enumerator API. Each element performs two separate GetComponent<SpriteRenderer> calls: capture alpha from the first renderer's color, then apply the computed RGB and captured alpha to the second renderer. Do not consolidate these component lookups or introduce Unity truthiness checks. A null element/renderer raises normally. Dispose the captured enumerator on normal completion and on exceptions; acquisition occurs before the protected region.

## Device.LoopAddAnimation (0308)

Call Component.GetComponent<SpriteRenderer> on the supplied Transform and apply Unity truthiness to that result. If true, capture this.animationSprites before invoking suppliedTransform.gameObject, then append to that captured list. Enumerate Transform.GetEnumerator through the actual non-generic IEnumerator interface. Each Current is cast to Transform, including normal null propagation, then recursively calls the same method. The finally casts the captured enumerator to IDisposable and disposes it only when that cast is non-null. Preserve recursion, enumerator/interface dispatch, cast failure timing and disposal. Do not replace it with transform childCount/GetChild indexing or silently skip invalid Current values.

## Zombie portals (048F, 0494)

Capture map and deviceManager from the same initial board. Capture devicePrefabList[13] before the selection calls. Original metadata slot 181BAF660 closes Map.RandomGet_Grid_TestPlace with Device; original slots 181BACC90/181BC3778/181BC37D0 close the list/enum methods with Grid.

For 048F, the requested count is 2 + (brokenLevel < 2 ? 1 : 0), using signed comparison and reading brokenLevel before GetMapX. The remaining arguments are captured prefab, 4 and capturedMap.GetMapX()-2. For 0494, call GetMapX twice on the captured map; the selection arguments are 999, prefab, firstResult-1 and secondResult-1. Both calls remain observable.

Acquire List<Grid>.Enumerator before the try. For each Current allocate DeviceData(13), write the current zombie camp into it, read current Grid.gridX/gridY and call the captured manager.PlaceDevice. Apply Unity inequality to the returned device; only its true branch writes device.targetZombie=this. The 0494 variant then always invokes DC_PortalSummonFlag on the returned receiver, even when Unity equality classified a destroyed object as false. A raw null therefore still throws at that call. Both variants dispose the original captured enumerator through finally, including failures after earlier device placements. Existing helpers remain dependencies with their own acceptance status.

## Zombie.ZC_SPHTryShooting (04B1)

Re-read UI[0] for SetActive(shootReady), then for transform.position=the Vector3 argument by value. If Unity inequality accepts the current animator and its GetCurrentAnimatorClipInfo(0) array is non-empty, read current animator state for layer 0 and capture normalizedTime through the public AnimatorStateInfo getter. Fetch UI[0].transform.GetChild(1).GetComponent<Image>; set fillAmount to the float remainder normalizedTime % 1. Fetch UI[0].GetComponent<Animator> separately, apply Unity inequality and, on its true branch, call Play(0,0,capturedTime). Re-read shootReady, chargeReady and shooting at the common trigger tail. Only shootReady && chargeReady && !shooting invokes current animator.SetTrigger("ShootingTrigger"), then sets chargeReady=false and shooting=true. Preserve array and receiver exceptions and all callback mutation points. No synthetic MathF API is introduced.

## Plant.ST_Potatomortar (03AC)

Capture the current animation sprite position for the original Potatomortar_shell3 literal. Prefer the current targetZombie under Unity inequality and call GetPredictedPosition(1.05); otherwise use the current targetDevice transform position under its own Unity inequality. If neither branch is taken, retain the existing targetPosition. Assign the full Vector3 when a new target exists. Capture that full targetPosition and compute each velocity component (target-start)/float32(1.05), preserving evaluation order and float operations.

Create projectile ID 15 through current board.projectileManager. Original metadata slot 181BBB878 closes the seven-argument Projectile.Initial overload with Plant. Pass captured start, computed velocity, Vector3.zero, 50f, 1012.5f, 1 and this, using value-type Vector3 arguments. Obtain this.GetDamage(projectile,15,false). If current supperHit is true and captured current skill.ID==1, invoke that captured skill.Function(this,damage) and clear supperHit. Then projectile.SetDamage(damage,current camp), hide the current shell sprite, capture the current parameter_ints list, and decrement index 0 on that same captured list. Do not redirect the write if a callback replaces this.parameter_ints after the list is captured. Preserve side effects before later exceptions.

## Source delivery boundary

Four new finally regions are declared for 0352/0308/048F/0494. The aggregate preserves the prior 30 regions and all prior source bodies. Metadata additions are four MemberRefs, five local signatures and two closed MethodSpecs; no TypeSpec or user-string addition. Primary authoring performs no test, acceptance or workflow-result inspection. AGY owns candidate construction, isolation, static flow, member/API resolution, native/CIL behavior tests, meaningful mutants, regression and compiler execution through Actions.
