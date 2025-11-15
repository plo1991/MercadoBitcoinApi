# Integração com API do Mercado Bitcoin (C# .NET)

Este projeto implementa uma integração completa com a API v4 do Mercado Bitcoin, permitindo consultar informações sobre contas e posições de ativos com suporte a filtros por data.

## 📋 Funcionalidades

- ✅ Conexão autenticada com a API do Mercado Bitcoin
- ✅ Autenticação HMAC-SHA512 (TAPI) com formato hexadecimal
- ✅ Consulta de contas do usuário
- ✅ Consulta de posições de ativos por conta
- ✅ Filtros de consulta por data (início e fim) com validação
- ✅ Código estruturado e bem organizado
- ✅ Tratamento de erros robusto com mensagens detalhadas
- ✅ Thread-safe para uso em ambientes concorrentes
- ✅ Validação de parâmetros de entrada

## 🏗️ Estrutura do Projeto

```
MercadoBitcoinApi/
├── Models/
│   ├── Account.cs          # Modelo de conta
│   ├── Position.cs         # Modelo de posição de ativo
│   └── ApiResponse.cs      # Resposta genérica da API
├── Services/
│   ├── IAuthenticationService.cs    # Interface de autenticação
│   ├── AuthenticationService.cs     # Implementação HMAC-SHA512
│   ├── IMercadoBitcoinService.cs    # Interface do serviço principal
│   └── MercadoBitcoinService.cs     # Implementação do cliente HTTP
└── Program.cs              # Exemplo de uso
```

## 🔑 Configuração

### Pré-requisitos

- .NET 10.0 SDK ou superior (compatível com .NET 8.0+)
- Credenciais da API do Mercado Bitcoin (TAPI-ID e TAPI-SECRET)

### Como obter as credenciais

1. Acesse sua conta no Mercado Bitcoin
2. No menu superior, clique no seu nome e selecione "Configurações"
3. No menu lateral, em "Integrações", selecione "Chave de API"
4. Clique em "Nova Chave", preencha os campos e confirme com o código 2FA
5. Guarde o **TAPI-ID** e o **TAPI-SECRET** gerados

## 🚀 Como Usar

### 1. Compilar o projeto

```bash
dotnet build
```

### 2. Executar o exemplo

```bash
dotnet run --project MercadoBitcoinApi
```

O programa solicitará as credenciais (TAPI-ID e TAPI-SECRET) e então:
- Listará todas as contas disponíveis
- Consultará as posições da primeira conta
- Demonstrará o uso de filtros por data

### 3. Usar em seu próprio código

```csharp
using MercadoBitcoinApi.Services;

// Configurar serviços
var httpClient = new HttpClient();
var authService = new AuthenticationService("seu-tapi-id", "seu-tapi-secret");
var mercadoBitcoinService = new MercadoBitcoinService(httpClient, authService);

// Obter contas
var accounts = await mercadoBitcoinService.GetAccountsAsync();

// Obter posições sem filtro
var positions = await mercadoBitcoinService.GetPositionsAsync("account-id");

// Obter posições com filtro de data
var startDate = DateTime.Now.AddDays(-30);
var endDate = DateTime.Now;
var filteredPositions = await mercadoBitcoinService.GetPositionsAsync(
    "account-id",
    startDate: startDate,
    endDate: endDate
);
```

## 🔒 Segurança

⚠️ **IMPORTANTE**: Nunca compartilhe ou commite suas credenciais (TAPI-SECRET) no código. Em produção, utilize:

- Variáveis de ambiente
- Azure Key Vault
- AWS Secrets Manager
- Outros gerenciadores de segredos

## 📚 Documentação da API

A documentação oficial da API do Mercado Bitcoin está disponível em:
https://api.mercadobitcoin.net/api/v4/docs

## 🛠️ Tecnologias Utilizadas

- .NET 10.0
- HttpClient (para requisições HTTP)
- Newtonsoft.Json (para serialização/deserialização JSON)
- HMAC-SHA512 (para autenticação)

## 📝 Notas de Implementação

### Autenticação HMAC-SHA512

A autenticação é realizada através de:
- **TAPI-ID**: Identificador da chave de API
- **TAPI-NONCE**: Número único incremental para cada requisição (thread-safe)
- **TAPI-MAC**: Assinatura HMAC-SHA512 calculada sobre a mensagem da requisição

A mensagem assinada segue o formato: `{method}{path}{queryString}{body}{nonce}`

O MAC é gerado em hexadecimal (lowercase), conforme especificação da API v4 do Mercado Bitcoin.

### Filtros de Data

Os filtros de data são opcionais e podem ser usados para:
- Consultar posições em um período específico
- Reduzir a quantidade de dados retornados
- Melhorar a performance das consultas

**Validação**: O sistema valida automaticamente que a data inicial não seja posterior à data final, lançando uma exceção `ArgumentException` caso contrário.

### Tratamento de Erros

O serviço implementa tratamento robusto de erros:
- Captura e relança exceções HTTP com detalhes da resposta da API
- Trata erros de deserialização JSON separadamente
- Fornece mensagens de erro claras e informativas
- Valida parâmetros de entrada antes de fazer requisições

## 🤝 Contribuindo

Este é um projeto de exemplo/teste. Sinta-se à vontade para sugerir melhorias ou correções.

## 📄 Licença

Este projeto é fornecido como exemplo educacional.

