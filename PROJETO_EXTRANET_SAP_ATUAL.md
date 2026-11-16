# 📚 DOCUMENTAÇÃO COMPLETA - ExtranetSAP (Projeto Original)

**Data:** 2026-11-11  
**Propósito:** Preservar estrutura completa para referência durante reorganização

---

## 🏗️ ARQUITETURA (Clean Architecture)

```
ExtranetSAP/
├── Domain/              # Entidades, DTOs, Interfaces
├── Application/         # Casos de Uso (Handlers), Serviços
├── Persistence/         # Repositórios, EF Core, Seeders, SAP
├── WebAPI/              # Controllers REST API (22 controllers)
└── WebFrontEnd/         # Blazor WebAssembly
```

---

## 📦 MÓDULOS FUNCIONAIS

### 1. Autenticação ✅
- Login JWT + 2FA
- Registo com confirmação email
- Reset password
- Perfil utilizador

### 2. Catálogo SAP B1 ✅
- Items (artigos)
- ItemGroups
- ItemExtensions
- Carrinho persistente

### 3. Documentos SAP ✅
- Orders (encomendas)
- Quotations (cotações)
- Invoices (faturas)
- CreditNotes
- DeliveryNotes
- IncomingPayments
- Returns

### 4. Dashboard ✅
- KPIs vendas
- Métricas

### 5. Licenças ⚠️
- ✅ UI completa (IndexLicenses.razor)
- ⚠️ Backend parcial (só leitura via ClientLicenseApiClient)
- ❌ Falta: POST /api/Licenses/request
- ❌ Falta: POST /api/Licenses/{id}/renew

### 6. Configurações ✅
- AppSettings
- BusinessPartners
- Warehouses
- VatGroups
- SQL Queries

---

## 🎨 LAYOUT E DESIGN (MANTER)

### Framework Visual
- **AdminLTE 3** ⭐
- **Bootstrap 5**
- **Font Awesome**
- **Litepicker**

### Componentes Reutilizáveis (WebFrontEnd/Pages/Components/Geral/)
```
✅ AppContentHeader.razor       # Cabeçalho páginas
✅ AppTableComponent.razor       # Tabela genérica
✅ AppTableDetails.razor         # Detalhes tabela
✅ AppPageDetailsLayout.razor    # Layout detalhes
✅ AppTabs.razor                 # Sistema tabs
✅ LoadingPage.razor             # Loading spinner
✅ Toast.razor                   # Notificações
✅ ConfirmModal.razor            # Modal confirmação
✅ DatePicker.razor              # Date picker
✅ MultiSelect.razor             # Multi select
✅ CustomSelect.razor            # Select custom
✅ CartDropdownComponent.razor   # Dropdown carrinho
✅ SidebarMenuComponent.razor    # Menu lateral
✅ CultureSelector.razor         # Seletor idioma
✅ AppField.razor                # Campo formulário
✅ BusyButtonComponent.razor     # Botão com loading
✅ GridOptionsComponent.razor    # Opções grelha
✅ HeaderDetailsComponent.razor  # Cabeçalho detalhes
✅ MessageDialog.razor           # Diálogo mensagem
✅ ModalChooseGlobal.razor       # Modal escolha
```

### Tema
- Dark Mode ✅
- Light Mode ✅
- Preferência por utilizador

---

## 🔌 INTEGRAÇÕES EXTERNAS

### 1. SAP Business One Service Layer
**Localização:** `Persistence/SAP_Helps/`
- `SapB1ServiceLayerClient.cs`
- `SapSessionHandler.cs`
- `SapUserImporter.cs`

**Endpoints:**
- `/b1s/v1/Items`
- `/b1s/v1/Orders`
- `/b1s/v1/Invoices`
- `/b1s/v1/BusinessPartners`
- `/b1s/v1/SQLQueries(...)`

### 2. LicenseKey API
**Localização:** `Application/Services/ClientLicenseApiClient.cs`
- `GET api/client/licenses/{cardCode}`
- Autenticação: `X-Api-Key` header

### 3. Leads API
**Localização:** `Application/Services/LeadApiClient.cs`
- `POST api/leads` (criação contas/trials)

---

## 🌐 API REST - 22 Controllers

| Controller | Rota | Funcionalidade |
|------------|------|----------------|
| UsersController | /api/Users | Utilizadores |
| ItemsController | /api/Items | Artigos SAP |
| OrdersController | /api/Orders | Encomendas SAP |
| OrdersCartController | /api/OrdersCart | Carrinho |
| InvoicesController | /api/Invoices | Faturas SAP |
| QuotationsController | /api/Quotations | Cotações SAP |
| CreditNotesController | /api/CreditNotes | Notas crédito SAP |
| DeliveryNotesController | /api/DeliveryNotes | Guias SAP |
| IncomingPaymentsController | /api/IncomingPayments | Pagamentos SAP |
| ReturnsController | /api/Returns | Devoluções SAP |
| DashboardController | /api/Dashboard | Métricas |
| BusinessPartnersController | /api/BusinessPartners | Parceiros SAP |
| ItemGroupsController | /api/ItemGroups | Grupos artigos |
| ItemExtensionController | /api/ItemExtension | Extensões |
| WarehousesController | /api/Warehouses | Armazéns SAP |
| VatGroupsController | /api/VatGroups | IVA SAP |
| AppSettingsController | /api/AppSettings | Configs |
| Attachments2Controller | /api/Attachments2 | Anexos SAP |
| SQLQueriesController | /api/SQLQueries | Queries SQL |
| LeadsController | /api/Leads | Criação contas |
| onexClientLicensesController | /client/licenses | Licenças (leitura) |
| InternalOnexClientUsersController | /internal/onexClientUsers | Users Onex |

---

## 🖥️ PÁGINAS FRONTEND (Blazor)

### Autenticação (Auth/) - MANTER ✅
```
Login.razor
Register.razor
ConfirmEmail.razor
Verify2FA.razor
SetPassword.razor
ActivateAccount.razor
SettingUser.razor
AccessDenied.razor
AuthShell.razor
RedirectToLogin.razor
```

### Catálogo (Catalog/) - REMOVER ❌
```
CatalogPage.razor
ProductGrid.razor
ProductCard.razor
GroupList.razor
Pagination.razor
```

### Documentos SAP - REMOVER ❌
```
Orders/
Invoices/
Quotations/
CreditNotes/
DeliveryNotes/
IncomingPayments/
Returns/
```

### Licenças - MANTER E EXPANDIR ✅
```
Licenses/IndexLicenses.razor  ✅ UI completa
```

### Outros
```
HomePage/DashboardHomePage.razor     # Adaptar
Users/IndexUsers.razor               # Manter ✅
ItemExtensions/                      # Remover ❌
AppSettigs/IndexAppSettings.razor    # Manter ✅
```

### Layout (MANTER TUDO) ✅
```
Layout/MainLayout.razor
Layout/NavMenu.razor
Layout/EmptyLayout.razor
```

---

## 🗄️ BASE DE DADOS

### Tecnologia
- SQL Server
- Entity Framework Core
- ASP.NET Identity

### Tabelas - MANTER
```sql
AspNetUsers          ✅
AspNetRoles          ✅
AspNetUserRoles      ✅
AspNetUserClaims     ✅
AspNetUserLogins     ✅
AspNetUserTokens     ✅
AspNetRoleClaims     ✅
```

### Tabelas - REMOVER
```sql
OrderCart            ❌
OrderLineCart        ❌
ItemExtension        ❌
```

### Tabelas - CRIAR
```sql
LicenseRequest       ✨ Nova
LicenseRenewal       ✨ Nova
```

---

## 🌍 INTERNACIONALIZAÇÃO - MANTER ✅

### Idiomas
- pt-PT (Principal)
- pt-BR
- en-US

### Arquivos Resources/
```
LicensesApp.pt-PT.resx   ✅
LicensesApp.pt-BR.resx   ✅
LicensesApp.en-US.resx   ✅
LeadsApp.*.resx          ✅
```

---

## 🔐 SEGURANÇA - MANTER TUDO ✅

### Autenticação
- JWT Bearer Token
- JwtTokenGenerator.cs

### Autorização
- Roles: Manager, Administrator, User
- [Authorize] attributes

### Validações
- Email único
- NUIT único
- Password forte (min 6 chars)
- Rate limiting

### CORS
- Configurado para frontend

---

## ⚙️ CONFIGURAÇÕES

### appsettings.json (WebAPI) - ADAPTAR
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "..."  ✅ Manter
  },
  "Jwt": { ... }                ✅ Manter
  "SapB1": { ... }              ❌ Remover
  "LicensingApi": { ... }       ✅ Manter
  "LeadsApi": { ... }           ✅ Manter
  "Email": { ... }              ✅ Manter
}
```

---

## 📝 ESTRUTURA DE PASTAS - O QUE FAZER

### MANTER ✅
```
Domain/
├── Models/
│   ├── AppUser.cs           ✅
│   └── AppRole.cs           ✅
├── Interfaces/
│   ├── IClientLicenseApiClient.cs  ✅
│   ├── IEmailService.cs            ✅
│   └── IJwtTokenGenerator.cs       ✅
└── Dtos/
    ├── ClientLicenseDto.cs         ✅
    └── ClientLicenseOverviewDto.cs ✅

Application/
├── UseCases/User/           ✅ Manter tudo
│   ├── Auth/
│   ├── CreateUser/
│   ├── DefinePassword/
│   ├── GetAllUser/
│   ├── UpdateUser/
│   └── Verify2FA/
├── UseCases/AppSettings/    ✅ Manter
└── Services/
    ├── ClientLicenseApiClient.cs  ✅
    └── LeadApiClient.cs           ✅

Persistence/
├── Seeders/
│   ├── IdentitySeeder.cs    ✅ Manter
│   └── AppConfigurationSeeder.cs  ✅ Manter
└── Migrations/              ✅ Manter

WebFrontEnd/
├── Layout/                  ✅ Manter tudo
├── Pages/Components/
│   ├── Auth/                ✅ Manter tudo
│   ├── Geral/               ✅ Manter tudo
│   ├── Licenses/            ✅ Manter e expandir
│   ├── Users/               ✅ Manter
│   └── AppSettigs/          ✅ Manter
├── Services/
│   ├── Auth/                ✅ Manter tudo
│   ├── Licenses/            ✅ Manter e expandir
│   └── Users/               ✅ Manter
└── wwwroot/
    ├── adminlte/            ✅ Manter
    ├── bootstrap/           ✅ Manter
    └── font-awesome/        ✅ Manter
```

### REMOVER ❌
```
Application/UseCases/
├── Items/                   ❌
├── Orders/                  ❌
├── Invoices/                ❌
├── Quotations/              ❌
├── CreditNotes/             ❌
├── DeliveryNotes/           ❌
├── IncomingPayments/        ❌
├── Returns/                 ❌
├── OrdersCarts/             ❌
├── ItemExtensions/          ❌
├── ItemGroups/              ❌
├── BusinessPartners/        ❌
├── Warehouses/              ❌
├── VatGroups/               ❌
├── SQLQueries/              ❌
├── Dashboard/               ❌ (ou adaptar)
└── Attachments2/            ❌

Persistence/
└── SAP_Helps/               ❌ Remover tudo
    ├── SapB1ServiceLayerClient.cs
    ├── SapSessionHandler.cs
    └── SapUserImporter.cs

Domain/
└── Models/
    ├── OrderCart.cs         ❌
    ├── OrderLineCart.cs     ❌
    └── ItemExtension.cs     ❌

WebAPI/Controllers/
├── ItemsController.cs       ❌
├── OrdersController.cs      ❌
├── OrdersCartController.cs  ❌
├── InvoicesController.cs    ❌
├── QuotationsController.cs  ❌
├── CreditNotesController.cs ❌
├── DeliveryNotesController.cs ❌
├── IncomingPaymentsController.cs ❌
├── ReturnsController.cs     ❌
├── ItemGroupsController.cs  ❌
├── ItemExtensionController.cs ❌
├── BusinessPartnersController.cs ❌
├── WarehousesController.cs  ❌
├── VatGroupsController.cs   ❌
├── SQLQueriesController.cs  ❌
├── Attachments2Controller.cs ❌
└── DashboardController.cs   ❌ (ou adaptar)

WebFrontEnd/Pages/Components/
├── Catalog/                 ❌
├── Orders/                  ❌
├── Invoices/                ❌
├── Quotations/              ❌
├── CreditNotes/             ❌
├── DeliveryNotes/           ❌
├── IncomingPayments/        ❌
├── Returns/                 ❌
├── ItemExtensions/          ❌
└── HomePage/                ❌ (ou adaptar)

WebFrontEnd/Services/
├── Items/                   ❌
├── Orders/                  ❌
├── Invoices/                ❌
├── Quotations/              ❌
├── IncomingPayments/        ❌
├── DeliveryNotes/           ❌
├── CreditNotes/             ❌
├── Returns/                 ❌
├── Warehouses/              ❌
└── VatGroups/               ❌
```

### CRIAR NOVO ✨
```
Application/UseCases/Licenses/
├── RequestLicense/          ✨
│   ├── RequestLicenseHandler.cs
│   ├── RequestLicenseRequest.cs
│   └── RequestLicenseResponse.cs
├── RenewLicense/            ✨
│   ├── RenewLicenseHandler.cs
│   ├── RenewLicenseRequest.cs
│   └── RenewLicenseResponse.cs
└── GetLicenses/             ✨
    ├── GetLicensesHandler.cs
    └── GetLicensesQuery.cs

WebAPI/Controllers/
└── LicensesController.cs    ✨ Novo (3 endpoints)

Domain/Models/
├── LicenseRequest.cs        ✨ (se necessário)
└── LicenseRenewal.cs        ✨ (se necessário)
```

---

## 🎯 CHECKLIST REORGANIZAÇÃO

### Fase 1: Backup e Preparação
- [x] Documentar projeto atual
- [ ] Criar branch: `reorganize-project`
- [ ] Commit atual antes de apagar

### Fase 2: Remover SAP
- [ ] Apagar controllers SAP (13 controllers)
- [ ] Apagar UseCases SAP (15 pastas)
- [ ] Apagar páginas frontend SAP (7 pastas)
- [ ] Apagar serviços frontend SAP (10 pastas)
- [ ] Apagar SAP_Helps/
- [ ] Apagar models SAP (OrderCart, ItemExtension)
- [ ] Remover configs SapB1 do appsettings

### Fase 3: Implementar Licenças
- [ ] Criar LicensesController completo
- [ ] Criar UseCases: RequestLicense, RenewLicense, GetLicenses
- [ ] Expandir ClientLicenseApiClient se necessário
- [ ] Testar endpoints

### Fase 4: Limpar e Ajustar
- [ ] Atualizar NavMenu.razor (remover links SAP)
- [ ] Atualizar Sidebar.razor
- [ ] Limpar migrations antigas (opcional)
- [ ] Atualizar README

### Fase 5: Testes
- [ ] Compilar projeto
- [ ] Testar login
- [ ] Testar página licenças
- [ ] Testar requisição licença
- [ ] Testar renovação licença

---

## 🚀 COMANDOS ÚTEIS

### Arrancar Projeto
```bash
# Backend
cd WebAPI
dotnet run --launch-profile https  # Porta 7118

# Frontend
cd WebFrontEnd
dotnet run --launch-profile https  # Porta 7083
```

### Base de Dados
```bash
cd WebAPI
dotnet ef database update
dotnet ef migrations add <nome>
```

---

## 📊 ESTATÍSTICAS PROJETO ATUAL

- **Controllers:** 22
- **Use Cases:** ~80 handlers
- **Páginas Frontend:** ~65 .razor
- **Componentes Reutilizáveis:** ~25
- **Serviços Frontend:** ~15
- **Idiomas:** 3 (pt-PT, pt-BR, en-US)
- **Integrações:** 3 (SAP, LicenseKey, Leads)

---

## 🎨 CORES E TEMA (AdminLTE)

### Cores Principais
```css
Primary:   #007bff (azul)
Success:   #28a745 (verde)
Danger:    #dc3545 (vermelho)
Warning:   #ffc107 (amarelo)
Info:      #17a2b8 (ciano)
Dark:      #343a40 (cinza escuro)
```

### Dark Mode
- Background: `#343a40`
- Borders: `border-white`
- Buttons: `btn-outline-light`

---

**FIM DA DOCUMENTAÇÃO**

Esta é a estrutura completa do ExtranetSAP antes da reorganização.
Use este documento como referência durante todo o processo de migração.
