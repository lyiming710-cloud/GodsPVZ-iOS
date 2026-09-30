# Native23 batch 2 -- precise checkpoint

branch: `repair/codex-native19-ipa`
parent commit: `09dd564962f6b83b3175194db97d95cb7e5346b9`

## what this commit contains

* the repair / verification / execution scripts listed below
* the six evidence logs produced by them
* this manifest

The DLL artefacts themselves live in the git-ignored `.validation/`
tree; their SHA-256 values are recorded here so the run can be matched.

## artefact hashes

```
4d970f5ed7e63dd42815c3c843ce51918328ff790fa5d1eb77e2a80e6f3b0a1b  work/batch1.dll
1d8e743df3407b3769f50885a55556ae380525595b4284ad223795af0aa22e5d  work/batch2.dll
6e8caffc6188d1f93a72d999f955056836c6cc263e21cda8cad49222191d3c79  work/batch2a.dll
3b5226b9407bb556b2a165af6f146fe0d741eff2305830c312439943159f7b79  edits-batch2.json
a102f60b04832470014575d26b5bc69f71fb0761abdf017c1fed60404e62aeb0  edits-batch2a.json
36f892be52f897994119057a03d377cb887421e79ad674bb87931dbf649c729a  out/afam2-candidates.json
2f959a791a7924297cac699abcbee8350525d4c974aadd310e73de3712e67291  out/afam3-accepted.json
70241fc43ab4f98849aa547565886327b1d0ccacb6ac93c26079f5ff831929a9  out/afam3-quarantine.json
04e15a30c9eb667217a459f9d20dd9a71414b13b83e044ba12ec01c7d6da9734  out/behave-sites.json
32f8d7c2199e0bf38367fc9bece364365eceef5777358143b8400b56b277a06a  out/tiers.json
9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d  pcnative/GameAssembly.dll
```

## evidence tiers (see audit/native23/tiers.log)

| tier | meaning | methods |
|---|---|---|
| E1 | typed oracle FAIL->PASS, every flip individually necessary | 377 |
| E2 | reference type vouched for by a metadata signature | 249 |
| E3 | concrete execution reached every accepted site | 198 |
| E4 | PC-native body located and corroborates at method level | 362 |
| E5 | null-check site has site-level native evidence | 3 |
| E6 | whole-method behaviour proven | 0 |

## the claim that must NOT be over-read

377 is the number of methods TOUCHED. It is not a number of methods
PROVEN. Only E6 would be, and E6 = 0.
