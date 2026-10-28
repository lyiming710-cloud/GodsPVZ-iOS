# R22 actual complex gameplay and Apple evidence

Unity run 40456165422 at d707440 used exact candidate 15e01503. Every delivered
gameplay byte was checked against its delivery manifest. All 1,598,357 trajectory
records were read. The raw text, original PNGs, runtime support DLLs and their
manifests are preserved here, including the failure.

The original Adventure 19 began through OS selection/confirm/start clicks.
35 zombies were observed; three active zombies changed position and four had a
nonempty path in at least one sample. These observations do not establish correct
path geometry. No plant was placed and no plant-origin projectile was observed.
The game advanced 12.67876 seconds after board start before a normal original
path-warning modal paused it. The input helper did not handle that modal and
ended with WALL_TIMEOUT_RETAINED. Device.Update produced 165,880 InvalidProgram
events; Device.InjuryStatusUpdate also threw twice. Original Plant/Zombie resource
readback still passed all 9,902 checks over 70 routes. The Personal seat was returned.

Independent investigation found that R22 incorrectly used Animator.SetTrigger
where original AnimationModule MethodDef 0x06000016 / native 0x1812dba00 is
Animator.SetBool(string,bool), with false in r8. The original CardBank controller
declares hide as a bool defaulting to true. The old finite oracle incorrectly
labeled that address as a three-argument trigger leaf and failed to observe the
bool argument. The old report remains historical evidence; its Board.Update
comparison is not sufficient semantic proof. A corrected ABI/state oracle finds
24 genuine normal-path differences in the old R22 DLL. The corrected child lives
in the separate Startup-R23-Runtime-Correction checkpoint.

Apple run 40456172986 independently rehashed all 755 C++ and 274 C files and read
all 1,029 raw logs: zero errors, 10,572 warnings. Six compiler controls and all
34 errors in the known damaged R4 source were read back. This used previously
captured physical-iOS runtime headers and portable newly generated sources; it
did not export/link this candidate, generate an IPA, or pass real gameplay.
