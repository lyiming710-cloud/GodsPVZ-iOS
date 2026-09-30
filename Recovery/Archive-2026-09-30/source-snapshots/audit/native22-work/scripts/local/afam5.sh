cd /workspaces/GodsPVZ-native19/.validation/native22
if [ -f out/afam2-candidates.json ]; then echo DONE; else echo RUNNING; fi
wc -c afam2.out.txt
tail -c 800 afam2.out.txt
