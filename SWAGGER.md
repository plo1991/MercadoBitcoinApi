# 📚 Documentação Swagger - Mercado Bitcoin API

## 🎯 Acessar o Swagger UI

Após executar a aplicação, o Swagger UI estará disponível em:

- **Desenvolvimento**: `http://localhost:5000` ou `https://localhost:5001`
- **Produção**: `https://seu-dominio.com`

## 🚀 Como Executar com Swagger

### 1. Configurar as Credenciais

Antes de executar, configure as credenciais da API do Mercado Bitcoin:

#### Opção A: Via appsettings.json

Edite o arquivo `appsettings.json` ou `appsettings.Development.json`:

```json
{
  "MercadoBitcoin": {
    "TapiId": "seu-tapi-id",
    "TapiSecret": "seu-tapi-secret"
  }
}
```

#### Opção B: Via Variáveis de Ambiente

```powershell
$env:TAPI_ID = "seu-tapi-id"
$env:TAPI_SECRET = "seu-tapi-secret"
```

### 2. Executar a Aplicação

```powershell
dotnet run --project MercadoBitcoinApi
```

Ou no Visual Studio:
- Pressione **F5** ou clique em **Start**

### 3. Acessar o Swagger

O navegador abrirá automaticamente no Swagger UI. Se não abrir, acesse:
- `http://localhost:5000` (HTTP)
- `https://localhost:5001` (HTTPS)

## 📋 Endpoints Disponíveis

### GET /api/MercadoBitcoin/accounts

Obtém todas as contas do usuário.

**Resposta de Sucesso (200):**
```json
[
  {
    "id": "abc123",
    "name": "Conta Principal",
    "type": "spot"
  }
]
```

### GET /api/MercadoBitcoin/accounts/{accountId}/positions

Obtém as posições de uma conta específica.

**Parâmetros:**
- `accountId` (path, obrigatório): ID da conta
- `startDate` (query, opcional): Data inicial (formato: yyyy-MM-dd)
- `endDate` (query, opcional): Data final (formato: yyyy-MM-dd)

**Exemplo de Requisição:**
```
GET /api/MercadoBitcoin/accounts/abc123/positions?startDate=2025-01-01&endDate=2025-11-14
```

**Resposta de Sucesso (200):**
```json
[
  {
    "asset": "BTC",
    "quantity": 0.5,
    "available_quantity": 0.3,
    "locked_quantity": 0.2,
    "average_price": 250000.00,
    "updated_at": "2025-11-14T16:00:00Z"
  }
]
```

## 🧪 Testar no Swagger UI

1. Abra o Swagger UI no navegador
2. Expanda o endpoint desejado
3. Clique em **"Try it out"**
4. Preencha os parâmetros (se necessário)
5. Clique em **"Execute"**
6. Veja a resposta na seção **"Responses"**

## 🔒 Segurança

⚠️ **IMPORTANTE**: 
- Nunca commite o arquivo `appsettings.json` com credenciais reais
- Use variáveis de ambiente em produção
- O arquivo `appsettings.Development.json` está no `.gitignore` por padrão

## 📝 Documentação XML

A documentação XML é gerada automaticamente a partir dos comentários XML no código. Os comentários aparecem no Swagger UI como descrições dos endpoints e parâmetros.

## 🛠️ Personalizar o Swagger

Para personalizar o Swagger, edite o arquivo `Program.cs` na seção `AddSwaggerGen`:

```csharp
builder.Services.AddSwaggerGen(options =>
{
    // Adicione suas personalizações aqui
    options.IncludeXmlComments(xmlPath);
});
```

## 📚 Recursos Adicionais

- [Documentação do Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
- [Documentação da API do Mercado Bitcoin](https://api.mercadobitcoin.net/api/v4/docs)

