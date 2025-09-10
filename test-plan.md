# Test Plan - TodoApi

**Project:** TodoApi CRUD REST Service  
**Date:** 10. September 2025  
**Authors:** Oskar, Peter, Yusuf  

## 1. Introduktion og Formål

Denne testplan definerer vores systematiske tilgang til kvalitetssikring af TodoApi - en RESTful webservice implementeret i .NET 8. Planen sikrer omfattende test coverage gennem unit tests, integration tests og performance tests for at validere funktionalitet, pålidelighed og ydeevne.

### Målgruppe
- Udviklere på TodoApi projektet
- QA-personale der udfører tests
- Undervisere der evaluerer software kvalitet

## 2. Test Scope

**System Under Test:**
- TodoApi REST endpoints (GET, POST, PUT, DELETE /tasks)
- InMemoryTaskRepository data layer
- TaskItem model og validation logic
- Error handling og HTTP status codes
- Concurrent access patterns

**Out of Scope:**
- Authentication og authorization (ikke implementeret)
- Database persistence (kun in-memory storage)
- External API integrations
- Security testing

## 3. Test Strategy og Approach

### Test Pyramid Implementation
Vi følger test pyramid princippet med følgende distribution:
- **70% Unit Tests**: Hurtig feedback, isoleret komponent testing
- **25% Integration Tests**: End-to-end API validation  
- **5% Performance Tests**: Load og stress testing

### Test Philosophy
- **Risk-based testing**: Focus på kritisk funktionalitet først
- **Fail-fast approach**: Tidlig opdagelse af defects
- **Automation-first**: Alle tests er automatiserede for CI/CD
- **Clean test isolation**: Hver test kører uafhængigt

## 4. Test Categories og Coverage

### 4.1 Unit Tests
**Formål**: Teste isoleret funktionalitet i repository-laget.  
**Framework**: MSTest  
**Location**: `TodoApi.Tests/TasksServiceTests.cs`

**Test Cases**:
- Task creation med automatisk ID-generering
- Retrieve all tasks (empty og populated scenarios)
- Update eksisterende task funktionalitet
- Delete task og verification
- Edge cases: Update/delete non-existent tasks

### 4.2 Integration Tests
**Formål**: Validere complete HTTP request/response cycles.  
**Framework**: MSTest med WebApplicationFactory  
**Location**: `TodoApi.Tests/TasksApiTests.cs`

### API Endpoints Coverage
| Endpoint | Scenarios | Expected Results | Test Priority |
|----------|-----------|------------------|---------------|
| POST /tasks | Valid creation | 201 Created + task object | High |
| POST /tasks | Invalid input (ingen titel) | 400 Bad Request | High |
| GET /tasks | Empty collection | 200 OK + empty array | Medium |
| GET /tasks | Populated collection | 200 OK + task array | High |
| GET /tasks/{id} | Existing task | 200 OK + task object | High |
| GET /tasks/{id} | Non-existing task | 404 Not Found | Medium |
| PUT /tasks/{id} | Update existing | 200 OK + updated task | High |
| PUT /tasks/{id} | Update non-existing | 404 Not Found | Medium |
| DELETE /tasks/{id} | Delete existing | 204 No Content | High |
| DELETE /tasks/{id} | Delete non-existing | 404 Not Found | Medium |

### 4.3 Performance Tests  
**Formål**: Verificere system stabilitet under load.  
**Framework**: Abstracta.JmeterDsl  
**Location**: `TodoApi.LoadTests/JMeterDslTests.cs`

**Load Test Scenarios**:
1. **Basic Load**: 10 users, 50 iterations (GET /tasks only)
2. **Mixed CRUD Load**: 100 users, 5 iterations (full CRUD cycle)

### Performance Acceptance Criteria
- P99 response time < 5 sekunder
- 0% error rate under normal load
- Throughput ≥ 500 requests/second (basic scenario)
- Memory usage remains stable during load

## 5. Test Environment og Setup

### Environment Requirements
- **.NET 8 SDK**: Core runtime environment
- **MSTest Framework**: Unit og integration test execution
- **Abstracta.JmeterDsl 0.8.0**: Performance testing capability
- **ReportGenerator**: Code coverage visualization
- **Visual Studio/VS Code**: Development environment

### Test Data Strategy
- **Unit Tests**: Minimal, synthetic data per test method
- **Integration Tests**: Fresh InMemoryTaskRepository per test run
- **Performance Tests**: Standardized JSON payloads for consistent measurements

**Sample Test Data**:
```json
{
  "id": 0,
  "title": "Test Task",
  "description": "Integration test description",
  "status": "CREATED"
}
```

### Prerequisites for Execution
- API skal køre på `http://localhost:5128` for performance tests
- Clean build environment uden cached artifacts
- Sufficient memory (8GB+) for concurrent load testing

## 6. Test Execution Procedures

### 6.1 Daily Development Testing
```bash
# Quick unit og integration tests
dotnet test .\TodoApi.Tests\
```

### 6.2 Code Coverage Analysis
```bash
# Generate coverage data
dotnet test .\TodoApi.Tests\ /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=TestResults\coverage.cobertura.xml /p:Threshold=80

# Create HTML coverage report
reportgenerator "-reports:.\TodoApi.Tests\TestResults\coverage.cobertura.xml" "-targetdir:.\TodoApi.Tests\TestResults\CoverageReport" -reporttypes:Html
```

### 6.3 Performance Testing
```bash
# Execute load tests (requires running API)
dotnet test .\TodoApi.LoadTests\ -c Release
```

### 6.4 Complete Test Pipeline
```bash
# Full validation sequence
dotnet build --configuration Release
dotnet test .\TodoApi.Tests\ /p:CollectCoverage=true
dotnet test .\TodoApi.LoadTests\ -c Release
```

## 7. Success Criteria og Quality Gates

### Functional Quality Gates
- ✅ **Test Execution**: Alle unit og integration tests skal pass
- ✅ **Code Coverage**: Minimum 80% line coverage (target: 100%)
- ✅ **API Compliance**: Alle HTTP status codes skal være korrekte
- ✅ **Error Handling**: Negative scenarios håndteres gracefully

### Performance Quality Gates  
- ✅ **Response Time**: P99 < 5 sekunder under normal load
- ✅ **Error Rate**: 0% fejl under load testing
- ✅ **Throughput**: ≥ 500 req/s for basic GET operations
- ✅ **Stability**: Ingen memory leaks eller crashes

### Documentation Quality
- ✅ **Test Coverage**: Uncovered code skal være dokumenteret
- ✅ **Defect Tracking**: Alle defects logged og resolved
- ✅ **Test Results**: Comprehensive execution reports

## 8. Test Schedule og Milestones

| Phase | Activity | Duration | Dependencies | Responsible |
|-------|----------|----------|--------------|-------------|
| **Setup** | Environment preparation | 0.5 dag | .NET 8 installed | Dev Team |
| **Unit Testing** | Repository layer tests | 2 dage | Code complete | Dev Team |
| **Integration** | API endpoint testing | 1.5 dage | Unit tests pass | QA Team |
| **Performance** | Load og stress testing | 1 dag | Integration stable | QA Team |
| **Reporting** | Coverage og summary reports | 0.5 dag | All tests complete | QA Lead |

**Total Estimated Effort**: 5.5 dage

### Key Milestones
- **M1**: Unit tests 100% pass rate
- **M2**: Integration tests cover alle endpoints  
- **M3**: Performance benchmarks achieved
- **M4**: Final test report delivered

## 9. Risk Management

### High-Risk Areas
- **Concurrency Issues**: In-memory repository under load
  - *Mitigation*: Targeted concurrent access tests
- **Data Consistency**: Task state mellem operations
  - *Mitigation*: Isolated test environments per test run
- **Performance Degradation**: Memory usage over time
  - *Mitigation*: Extended load testing med monitoring

### Test Environment Risks
- **Local Environment Dependency**: API på localhost:5128
  - *Mitigation*: Clear setup documentation og automation scripts
- **Tool Compatibility**: JMeter DSL version dependencies  
  - *Mitigation*: Verified tool versions i project files

### Contingency Plans
- **Critical Test Failures**: Halt release, immediate team notification
- **Performance Issues**: Isolate problem areas, targeted optimization
- **Environment Problems**: Fallback til clean development setup

## 10. Conclusion

Denne testplan sikrer comprehensive quality assurance for TodoApi gennem struktureret testing på multiple niveauer. Med focus på automatisering, coverage metrics og performance validation, etablerer vi en solid foundation for reliable software delivery og continuous integration practices.
