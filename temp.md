# Test Plan - TodoApi

**Project:** TodoApi CRUD REST Service  
**Date:** 10. September 2025  
**Authors:** Oskar, Peter, Yusuf  

## 1. Test Scope

**System Under Test:**
- TodoApi REST endpoints (GET, POST, PUT, DELETE /tasks)
- InMemoryTaskRepository data layer
- TaskItem model og validation logic

**Out of Scope:**
- Authentication (not implemented)
- Database persistence (in-memory only)

## 2. Test Strategy

Vi følger test pyramid med 70% unit tests, 25% integration tests, 5% performance tests.

### Test Types:
- **Unit Tests**: Repository layer functionality
- **Integration Tests**: HTTP endpoint validation  
- **Performance Tests**: Load testing med JMeter DSL

## 3. Test Coverage

### API Endpoints
| Endpoint | Scenarios | Expected Results |
|----------|-----------|------------------|
| POST /tasks | Valid creation, invalid input | 201 Created / 400 Bad Request |
| GET /tasks | Empty/populated collection | 200 OK with task array |
| GET /tasks/{id} | Existing/non-existing | 200 OK / 404 Not Found |
| PUT /tasks/{id} | Update existing/non-existing | 200 OK / 404 Not Found |
| DELETE /tasks/{id} | Delete existing/non-existing | 204 No Content / 404 Not Found |

### Performance Criteria
- P99 response time < 5 sekunder
- 0% error rate under load
- Concurrent user support (10-100 users)

## 4. Test Environment

**Requirements:**
- .NET 8 SDK
- MSTest framework
- Abstracta.JmeterDsl 0.8.0
- API running på localhost:5128

## 5. Test Execution

### Unit og Integration Tests
```bash
dotnet test .\TodoApi.Tests\
```

### Code Coverage
```bash
dotnet test .\TodoApi.Tests\ /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=TestResults\coverage.cobertura.xml /p:Threshold=80
reportgenerator "-reports:.\TodoApi.Tests\TestResults\coverage.cobertura.xml" "-targetdir:.\TodoApi.Tests\TestResults\CoverageReport" -reporttypes:Html
```

### Performance Tests
```bash
dotnet test .\TodoApi.LoadTests\ -c Release
```

## 6. Success Criteria

- ✅ Alle tests passed
- ✅ Code coverage ≥ 80%
- ✅ Performance benchmarks met
- ✅ No critical defects

## 7. Test Schedule

| Phase | Duration | Responsible |
|-------|----------|-------------|
| Unit Tests | 2 dage | Development Team |
| Integration Tests | 1 dag | QA Team |
| Performance Tests | 1 dag | QA Team |
| Reporting | 1 dag | QA Lead |

## 8. Risks

- **In-memory data loss**: Mitigated by fresh instances per test
- **Concurrent access issues**: Tested through load scenarios
- **Environment instability**: Quick setup scripts available
