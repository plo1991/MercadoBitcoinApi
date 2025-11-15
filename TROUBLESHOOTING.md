# 🔧 Solução de Problemas - Swagger

## Erro: "Failed to fetch"

Este erro geralmente ocorre quando o Swagger UI não consegue carregar o arquivo `swagger.json`. Siga estes passos:

### 1. Verificar se a aplicação está rodando

Certifique-se de que a aplicação está rodando e acessível:
```powershell
dotnet run --project MercadoBitcoinApi
```

### 2. Acessar a URL correta

- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`
- **Swagger JSON**: `http://localhost:5000/swagger/v1/swagger.json`

### 3. Verificar o console do navegador

Abra o DevTools (F12) e verifique:
- **Console**: Procure por erros de CORS ou rede
- **Network**: Verifique se a requisição para `/swagger/v1/swagger.json` está falhando

### 4. Limpar cache do navegador

Às vezes o cache do navegador pode causar problemas:
- Pressione `Ctrl + Shift + R` para recarregar forçando o cache
- Ou use uma janela anônima/privada

### 5. Verificar configuração de CORS

O CORS já está configurado no código. Se ainda houver problemas, verifique se está usando a URL correta.

### 6. Verificar se o Swagger está habilitado

Certifique-se de que está em modo **Development** ou que o Swagger está habilitado para o ambiente atual.

### 7. Verificar credenciais

Certifique-se de que as credenciais estão configuradas:
- Via `appsettings.Development.json`
- Ou via variáveis de ambiente `TAPI_ID` e `TAPI_SECRET`

## Erro: "URL scheme must be 'http' or 'https' for CORS request"

Este erro indica que há um problema com o esquema da URL. Soluções:

### Solução 1: Usar HTTP em desenvolvimento

Se estiver usando HTTPS e tiver problemas, tente usar apenas HTTP:
```json
// launchSettings.json
"applicationUrl": "http://localhost:5000"
```

### Solução 2: Aceitar certificado HTTPS

Se estiver usando HTTPS, pode ser necessário aceitar o certificado de desenvolvimento:
```powershell
dotnet dev-certs https --trust
```

## Erro: "TAPI_ID não configurado"

Configure as credenciais no `appsettings.Development.json`:
```json
{
  "MercadoBitcoin": {
    "TapiId": "seu-tapi-id",
    "TapiSecret": "seu-tapi-secret"
  }
}
```

Ou via variáveis de ambiente:
```powershell
$env:TAPI_ID = "seu-tapi-id"
$env:TAPI_SECRET = "seu-tapi-secret"
```

## Ainda com problemas?

1. **Limpar e reconstruir**:
   ```powershell
   dotnet clean
   dotnet restore
   dotnet build
   ```

2. **Verificar logs**: Os logs da aplicação podem mostrar mais detalhes sobre o erro

3. **Testar diretamente o endpoint JSON**:
   Acesse `http://localhost:5000/swagger/v1/swagger.json` diretamente no navegador
   - Se funcionar: o problema é no Swagger UI
   - Se não funcionar: o problema é na configuração do Swagger

4. **Verificar firewall/antivírus**: Pode estar bloqueando conexões locais

