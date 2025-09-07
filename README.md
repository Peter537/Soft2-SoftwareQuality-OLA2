# Soft2-SoftwareQuality-OLA2

## Group

- Oskar (Ossi-1337, cph-oo221)
- Peter (Peter537, cph-pa153)
- Yusuf (StylizedAce, cph-ya56)

## Test plan

Vores Test plan er i [test-plan.md](./test-plan.md)

## Reflektions dokument

Vores Reflektions dokument omkring code coverage og performance er i [reflection-document.md](./reflection-document.md)

## JMeter test plan

Vores JMeter test plan er i [jmeter-test-plan.md](./jmeter-test-plan.md)

## Kørsel af program

### Code Coverage

Code coverage kan køres med kommandoerne i CLI:

```bash
dotnet test .\TodoApi.Tests\TodoApi.Tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=TestResults\coverage.cobertura.xml /p:Threshold=80 /p:ThresholdType=line
```

```bash
reportgenerator "-reports:.\TodoApi.Tests\TestResults\coverage.cobertura.xml" "-targetdir:.\TodoApi.Tests\TestResults\CoverageReport" -reporttypes:Html
```

Hvor at HTML'en kan findes i `TodoApi.Tests\TestResults\CoverageReport\index.html`

### JMeter testing

JMeter load tests kan køres ved brug af CLI kommandoen:

```bash
dotnet test .\TodoApi.LoadTests\ -c Release
```

*OBS: Dette skal udføres fra roden af projektet.*
