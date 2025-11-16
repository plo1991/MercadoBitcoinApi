# MercadoBitcoinApi

API wrapper para integração com a API pública do Mercado Bitcoin. Fornece endpoints para autorização e consulta de contas/posições, além de serviços internos para autenticação e chamadas HTTP.

## Visão geral
- Projetado em .NET 10 (C# 14).
- Serviços principais:
  - `AuthenticationService` — realiza `POST /api/v4/authorize` e retorna `AuthResponse` (bearer token).
  - `MercadoBitcoinService` — consome endpoints com `Authorization: Bearer {token}` (`/accounts`, `/accounts/{id}/positions`).
- API expõe controladores em `api/mercadobitcoin`.

## Requisitos
- .NET 10 SDK
- Variáveis de ambiente ou segredos de usuário com credenciais do Mercado Bitcoin:
  - `TAPI_ID`
  - `TAPI_SECRET`

## Configuração
1. Configure variáveis de ambiente:
   - Windows PowerShell:
     - $env:TAPI_ID = "seu_tapi_id"
     - $env:TAPI_SECRET = "seu_tapi_secret"
   - Linux/macOS:
     - export TAPI_ID="seu_tapi_id"
     - export TAPI_SECRET="seu_tapi_secret"

2. Ou use segredos de usuário para desenvolvimento:
   - No Visual Studio: abra __Manage User Secrets__ do projeto e adicione:
     ```json
     {
       "MercadoBitcoin": {
         "TapiId": "<seu_tapi_id>",
         "TapiSecret": "<seu_tapi_secret>"
       }
     }
     ```
   - Ou via CLI:
     ```
     dotnet user-secrets init
     dotnet user-secrets set "MercadoBitcoin:TapiId" "<seu_tapi_id>"
     dotnet user-secrets set "MercadoBitcoin:TapiSecret" "<seu_tapi_secret>"
     ```

3. Certifique-se de registrar `HttpClient` no DI conforme `Program.cs`. Por padrão o projeto já registra `AddHttpClient` e os serviços.

## Executar localmente
- Build + run:

- Swagger UI:
  - Em desenvolvimento: disponível na raiz (`/`)
  - Em produção: `/swagger`

## Endpoints principais
- `POST /api/mercadobitcoin/authorize`
  - Body JSON: `{ "login": "...", "password": "..." }`
  - Retorna `AuthResponse` com `access_token` e `expiration`.

- `GET /api/mercadobitcoin/accounts`
  - Retorna lista de contas.
  - O controller obtém token internamente via `AuthenticationService` (não é necessário enviar token no request ao seu serviço).

- `GET /api/mercadobitcoin/accounts/{accountId}/positions?startDate=yyyy-MM-dd&endDate=yyyy-MM-dd`
  - Retorna posições da conta (com filtros opcionais).

Exemplo curl (autentica diretamente contra a API pública — apenas para referência):
curl -X POST "https://localhost:5001/api/mercadobitcoin/authorize" 
-H "Content-Type: application/json" 
-d '{"login":"<login>","password":"<password>"}'


## Testes
- Projeto de testes usa xUnit + Moq.
