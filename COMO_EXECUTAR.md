# 🚀 Como Executar o Projeto

## Pré-requisitos

- .NET 10.0 SDK instalado (ou superior)
- Visual Studio 2022 ou superior (opcional, para desenvolvimento)
- Credenciais da API do Mercado Bitcoin (TAPI-ID e TAPI-SECRET)

## 🎯 Executar no Visual Studio

### Abrir a Solução

1. Abra o Visual Studio 2022 ou superior
2. Clique em **File > Open > Project/Solution**
3. Navegue até a pasta do projeto e selecione o arquivo **`MercadoBitcoinApi.sln`**
4. Clique em **Open**

### Executar o Projeto

1. **Configure as credenciais** no arquivo `appsettings.Development.json` ou via variáveis de ambiente:
   ```json
   {
     "MercadoBitcoin": {
       "TapiId": "seu-tapi-id",
       "TapiSecret": "seu-tapi-secret"
     }
   }
   ```

2. No Visual Studio, certifique-se de que o projeto **MercadoBitcoinApi** está definido como projeto de inicialização
3. Pressione **F5** ou clique no botão **▶️ Start** (ou **Iniciar**)
4. O navegador abrirá automaticamente no **Swagger UI** (`http://localhost:5000` ou `https://localhost:5001`)
5. Use o Swagger UI para testar os endpoints da API

### Compilar a Solução

- Pressione **Ctrl+Shift+B** ou vá em **Build > Build Solution**

## 📝 Executar via Linha de Comando

### 1. Configurar as Credenciais

**Opção A: Via appsettings.json**

Edite `MercadoBitcoinApi/appsettings.Development.json`:
```json
{
  "MercadoBitcoin": {
    "TapiId": "seu-tapi-id",
    "TapiSecret": "seu-tapi-secret"
  }
}
```

**Opção B: Via Variáveis de Ambiente**

```powershell
$env:TAPI_ID = "seu-tapi-id"
$env:TAPI_SECRET = "seu-tapi-secret"
```

### 2. Executar o Projeto

```powershell
dotnet run --project MercadoBitcoinApi
```

### 3. Acessar o Swagger

O navegador abrirá automaticamente no Swagger UI. Se não abrir, acesse:
- `http://localhost:5000` (HTTP)
- `https://localhost:5001` (HTTPS)

### 4. Usar a API

Use o Swagger UI para:
- Ver todos os endpoints disponíveis
- Testar os endpoints diretamente no navegador
- Ver a documentação completa da API

## 📋 O que a API oferece?

A API Web com Swagger permite:

1. ✅ **GET /api/MercadoBitcoin/accounts** - Listar todas as contas disponíveis
2. ✅ **GET /api/MercadoBitcoin/accounts/{accountId}/positions** - Consultar posições de uma conta
3. ✅ **Filtros por data** - Consultar posições em um período específico
4. ✅ **Documentação interativa** - Testar endpoints diretamente no Swagger UI

## 📖 Exemplo de Uso no Swagger

1. **Acesse o Swagger UI** em `http://localhost:5000`
2. **Expanda o endpoint** `GET /api/MercadoBitcoin/accounts`
3. **Clique em "Try it out"**
4. **Clique em "Execute"**
5. **Veja a resposta** com a lista de contas

Para consultar posições:
1. Expanda `GET /api/MercadoBitcoin/accounts/{accountId}/positions`
2. Clique em "Try it out"
3. Preencha o `accountId` (ex: `abc123`)
4. Opcionalmente, adicione `startDate` e `endDate` (ex: `2025-01-01`)
5. Clique em "Execute"
6. Veja as posições retornadas

📚 **Consulte o arquivo `SWAGGER.md` para mais detalhes sobre o Swagger.**

## Executar a partir do diretório raiz

Se você estiver no diretório raiz do projeto (`Suno`), pode executar:

```powershell
# Usando a solução
dotnet build MercadoBitcoinApi.sln
dotnet run --project MercadoBitcoinApi

# Ou diretamente pelo projeto
dotnet run --project MercadoBitcoinApi
```

## Executar o executável compilado

Após compilar, você também pode executar diretamente o arquivo DLL:

```powershell
dotnet MercadoBitcoinApi/bin/Debug/net10.0/MercadoBitcoinApi.dll
```

## Solução de Problemas

### Erro: "Não foi possível encontrar o SDK"
- Certifique-se de que o .NET 10.0 SDK está instalado
- Execute `dotnet --version` para verificar

### Erro de autenticação
- Verifique se o TAPI-ID e TAPI-SECRET estão corretos
- Certifique-se de que as credenciais têm permissões para consultar contas e posições

### Erro de conexão
- Verifique sua conexão com a internet
- A API pode estar temporariamente indisponível

## Dicas

- 🔒 **Nunca compartilhe suas credenciais**
- 💾 Em produção, use variáveis de ambiente ou um gerenciador de segredos
- 📝 Consulte a documentação oficial: https://api.mercadobitcoin.net/api/v4/docs

