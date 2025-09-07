# Reflektions dokument

## Code Coverage

I arbejdet med code coverage begyndte vi med en afbalanceret tilgang mellem unit- og integrationstests. Unit-tests blev brugt til at dække den centrale forretningslogik i repository-laget, mens vi lagde en integrationstest per HTTP-endpoint for at validere endpointsnes happy path fungerede korrekt. Den første måling afslørede dog, at vores branch coverage lå under 60%, hvilket indikerede, at vores fejlhåndtering ikke blev udløst af de eksisterende tests.

![alt text](./img/code-coverage-1.png)

For at målrette indsatsen brugte vi ReportGenerator til at visualisere utestede linjer og conditionals. Det gjorde det let at identificere konkrete huller, som fx validering af tom eller manglende titel, 404-flows ved tasks, opdatering og sletning af ikke-eksisterende id’er samt negative scenarier med id=0. På den baggrund lavede vi flere integrationstests, der rammer hele controller-flowet og sikrer realistiske data- og statusforløb. Samtidig fastholdt vi unit-tests omkring repository’et for at holde logikken hurtig at verificere og let at vedligeholde.

![alt text](./img/code-coverage-2.png)

Målet var mindst 80% coverage som acceptkriterium, men givet projektets begrænsede omfang valgte vi at gå efter 100% line coverage. Vi er bevidste om, at høj coverage ikke i sig selv garanterer korrekthed, men det reducerer risikoen for utestet kernefunktionalitet, øger tilliden ved refaktorering og gør regressionsfejl lettere at opdage.

![alt text](./img/code-coverage-3.png)

## Load testing
