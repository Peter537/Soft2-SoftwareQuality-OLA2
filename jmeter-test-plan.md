

## JMeter Test Plan For Load Testing af TodoApi

Denne testplan er designet til at udføre load tests på TodoApi ved hjælp af JMeter DSL (Abstracta.JmeterDsl). Testplanen indeholder to hovedscenarier: et basis GET-scenarie og et mixed CRUD-scenarie.


### Formål:

- Verificere at TodoApi leverer stabil ydeevne under forskellige belastningsscenarier.
- Acceptkriterier inkluderer:
    - P99 svartid er under 5 sekunder.
    - Antal samples er større end 0 (ingen tom kørsel).

### Testmiljø:
- **Base-URL**: http://localhost:5128
- **Forudsætninger**:
    - API’et er oppe at køre på den angivne base-URL før teststart.
    - Java JDK 8+ eller nyere er installeret.
    - JMeter (Abstracta.JmeterDsl) version 0.8.0 er installeret.
- **Outputmapper til resultater**:
    - jtls/ (basis GET-scenarie)
    - jtls-mixed/ (CRUD-scenarie)
        
    *OBS: Mapperne bliver oprettet automatisk ved første kørsel. Under: path\to\Soft2-SoftwareQuality-OLA2\TodoApi.LoadTests\bin\Release\net8.0\{jtls eller jtls-mixed}*
    


### Testdesign
Testplanen består af to Thread Groups, der matcher DSL-tests:

1) **Basic_Get_Tasks_Load**:
    - Formål: Måle svartid og stabilitet for list endpoints.
    - Belastning: 10 users (threads), 50 iterationer pr. user, ramp-up 1s.
    - Flow:
        - GET `/tasks`
    - Resultater:
    
        JTL-skrives til `jtls/{date} {uuid}.jtl`.

2) **Mixed_100_Users_Crud**:
    - Formål: 
    
        Simulere blandet trafik med CRUD-operationer.
    
    - Belastning: 
        
        100 users (threads), 5 iterationer pr. user, ramp-up 1s.
    
    - Fælles headers:

        `Content-Type: application/json`

    - Flow pr. iteration:
        - GET `/tasks`
        - POST `/tasks` med payload:
            - `{"id":0,"title":"lt","description":"x","status":"CREATED"}`
        - PUT `/tasks/1` med payload:
            - `{"id":1,"title":"lt-upd","description":"x","status":"IN_PROGRESS"}`
        - DELETE `/tasks/1`
    - Resultater:
        
        JTL-skrives til `jtls-mixed/{date} {uuid}.jtl`.


**Kørsel af JMeter**:

JMeter load tests kan køres ved brug af CLI kommandoen:
```bash
dotnet test .\TodoApi.LoadTests\ -c Release
```
*OBS: Dette skal udføres fra roden af projektet.*
