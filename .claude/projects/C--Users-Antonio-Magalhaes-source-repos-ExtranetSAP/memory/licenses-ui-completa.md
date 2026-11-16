---
name: licenses-ui-completa
description: Interface de gestão de licenças já implementada no frontend - requisição, renovação, listagem com filtros
metadata:
  type: reference
---

# UI de Licenças (Frontend Completo)

## Localização
`WebFrontEnd/Pages/Components/Licenses/IndexLicenses.razor`

## Funcionalidades Implementadas ✅

### 1. Listagem de Licenças
- Tabela com todas as licenças do cliente
- Colunas: Produto, Key, Utilizadores, Status, Expiração, Ações
- Status com badges coloridos (ativo, expirando, expirado, pendente, suspenso)
- Cálculo automático de "dias restantes"
- Status "expiring" quando faltam ≤30 dias

### 2. Filtros
- **Pesquisa de texto** (produto, chave de licença)
- **Filtro por status:**
  - Todos
  - Ativo
  - Expirando (≤30 dias)
  - Expirado
  - Pendente (aguarda aprovação)
  - Suspenso

### 3. Modal de Requisição de Nova Licença ✅
**Campos:**
- Produto (texto, max 150 chars) - obrigatório
- Número de utilizadores (1-10000) - obrigatório
- Notas (textarea, max 500 chars) - opcional

**Validação client-side:**
- Produto obrigatório
- Utilizadores entre 1 e 10000

**Endpoint esperado:**
```
POST api/Licenses/request
{
  "product": "string",
  "users": number,
  "notes": "string?"
}
```

### 4. Modal de Renovação de Licença ✅
**Campos:**
- Período (dropdown): 3, 6, 12, 24 meses
- Notas (textarea, max 500 chars)

**Botão:** Só aparece para licenças "active", "expiring", "expired"

**Endpoint esperado:**
```
POST api/Licenses/{id}/renew
{
  "months": number,
  "notes": "string?"
}
```

### 5. Serviço Frontend
`WebFrontEnd/Services/Licenses/LicensesService.cs`

**Métodos:**
- `GetAllAsync()` → GET api/Licenses
- `RequestAsync(form)` → POST api/Licenses/request
- `RenewAsync(id, form)` → POST api/Licenses/{id}/renew

### 6. DTOs
`WebFrontEnd/Services/Licenses/Dto/LicenseDtos.cs`

**LicenseItemDto:**
- Id, Product, LicenseKey, Status
- StartDate, ExpiryDate, Users

**LicenseRequestFormDto:**
- Product, Users, Notes

**LicenseRenewFormDto:**
- Months, Notes

## Tema e Responsividade
- ✅ Suporta Dark Mode
- ✅ Design AdminLTE
- ✅ Responsivo (mobile-friendly)
- ✅ Loading states
- ✅ Empty states
- ✅ Toasts de sucesso/erro

## O Que Falta no Backend ❌

1. **LicensesController completo**
   - GET api/Licenses
   - POST api/Licenses/request
   - POST api/Licenses/{id}/renew

2. **UseCases:**
   - RequestLicenseHandler
   - RenewLicenseHandler
   - GetLicensesHandler

3. **Integração com LicenseKey API**
   - Endpoint para criar requisição
   - Endpoint para renovação

## Tradução (i18n)
Recursos em `WebFrontEnd/Resources/LicensesApp.*.resx`:
- pt-PT, pt-BR, en-US
- Todas as labels e mensagens traduzidas

## Links Relacionados
- [[extranet-sap-estrutura-original]] - Estrutura completa do projeto
- [[project_reorganizacao_plano]] - Plano para implementar backend

**Why:** Esta memória documenta que a UI de licenças está 100% implementada e funcional, faltando apenas os 3 endpoints da API. Evita retrabalho na UI.

**How to apply:** Ao implementar gestão de licenças, focar apenas no backend (controller + use cases). A UI já está pronta e testada, incluindo validações, modais, filtros e tradução.
