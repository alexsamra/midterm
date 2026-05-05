# Final
Atm application

## Unit Tests

```bash
dotnet test backend/tests/tests.csproj
```

```bash
dotnet test backend/tests/tests.csproj \
	/p:CollectCoverage=true \
	/p:CoverletOutput=backend/tests/TestResults/Coverage/ \
	/p:CoverletOutputFormat=cobertura \
	/p:Include="[model*]*%2c[api*]*" \
	/p:Exclude="[tests*]*" \
	/p:ExcludeByFile="**/obj/**%2c**/Program.cs" \
	/p:Threshold=90 \
	/p:ThresholdType=line \
	/p:ThresholdStat=total
```