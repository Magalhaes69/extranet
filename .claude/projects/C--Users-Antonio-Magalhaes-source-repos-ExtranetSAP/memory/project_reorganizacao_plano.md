---
name: extranet-reorganizacao-plano
description: Plano de reorganização do ExtranetSAP - remover SAP B1, manter layout AdminLTE, focar em gestão de licenças
metadata:
  type: project
---

# Plano de Reorganização ExtranetSAP

## Objetivo
Transformar o ExtranetSAP (portal SAP B1) num sistema de **gestão de licenças**, mantendo todo o layout e infraestrutura de autenticação.

## Fases de Reorganização

### Fase 1: Preparação ✅
- [x] Documentar estrutura atual completa
- [x] Guardar em memória para referência
- [ ] Criar branch `reorganize-project`
- [ ] Commit de segurança antes de apagar

### Fase 2: Remover Módulos SAP ❌
**Controllers a remover (13):**
- ItemsController, OrdersController, InvoicesController
- QuotationsController, CreditNotesController, DeliveryNotesController
- IncomingPaymentsController, ReturnsController
- ItemGroupsController, ItemExtensionController
- BusinessPartnersController, WarehousesController, VatGroupsController
- SQLQueriesController, Attachments2Controller

**UseCases a remover (15 pastas):**
- Items/, Orders/, Invoices/, Quotations/
- CreditNotes/, DeliveryNotes/, IncomingPayments/, Returns/
- OrdersCarts/, ItemExtensions/, ItemGroups/
- BusinessPartners/, Warehouses/, VatGroups/
- SQLQueries/, Attachments2/, Dashboard/

**Frontend a remover:**
- Catalog/, Orders/, Invoices/, Quotations/
- CreditNotes/, DeliveryNotes/, IncomingPayments/
- Returns/, ItemExtensions/, HomePage/

**Serviços frontend a remover:**
- Items/, Orders/, Invoices/, Quotations/
- IncomingPayments/, DeliveryNotes/, CreditNotes/
- Returns/, Warehouses/, VatGroups/

**Infraestrutura SAP:**
- Persistence/SAP_Helps/ (tudo)
- SapB1ServiceLayerClient.cs
- SapSessionHandler.cs
- SapUserImporter.cs

**Models:**
- OrderCart.cs, OrderLineCart.cs, ItemExtension.cs

**Configuração:**
- Remover secção "SapB1" do appsettings.json

### Fase 3: Implementar Gestão Licenças ✨

**Criar LicensesController:**
```csharp
// WebAPI/Controllers/LicensesController.cs
[Route("api/[controller]")]
public class LicensesController
{
    // GET api/Licenses - Lista licenças cliente
    // POST api/Licenses/request - Requisita nova
    // POST api/Licenses/{id}/renew - Renova existente
}
```

**Criar UseCases:**
- Application/UseCases/Licenses/
  - RequestLicense/ (Handler, Request, Response)
  - RenewLicense/ (Handler, Request, Response)
  - GetLicenses/ (Handler, Query)

**Frontend:**
- ✅ UI já existe: IndexLicenses.razor
- ✅ Serviço já existe: LicensesService.cs
- ✅ DTOs já existem: LicenseDtos.cs

### Fase 4: Ajustar Navegação

**Atualizar NavMenu.razor:**
- Remover: Catálogo, Encomendas, Faturas, Cotações, etc
- Manter: Dashboard (adaptar), Licenças, Utilizadores, Configurações
- Links principais:
  - 🏠 Home/Dashboard
  - 🔑 Licenças
  - 👥 Utilizadores
  - ⚙️ Configurações

**Atualizar Sidebar.razor:**
- Menu simplificado focado em licenças
- Remover referências a documentos SAP

### Fase 5: Limpar Base Dados

**Migrations:**
- Criar nova migration: `RemoveSapTables`
- Drop tables: OrderCart, OrderLineCart, ItemExtension
- Manter: AspNetUsers, AspNetRoles, AppSetting

**Seeds:**
- ✅ Manter IdentitySeeder (utilizador manager)
- ✅ Manter AppConfigurationSeeder
- ❌ Remover SapSqlQueryUserSeeder

### Fase 6: Testes Finais

**Verificar:**
- [ ] Projeto compila sem erros
- [ ] Login funciona (manager@app.co.mz)
- [ ] Página licenças carrega
- [ ] Requisição licença funciona
- [ ] Renovação licença funciona
- [ ] Navegação está correta
- [ ] Tema dark/light funciona

## O Que Manter Intacto ✅

### Layout AdminLTE
- `Layout/MainLayout.razor`
- `Layout/NavMenu.razor`
- `Layout/EmptyLayout.razor`
- Todo `wwwroot/adminlte/`
- Todo `wwwroot/bootstrap/`
- Todo `wwwroot/font-awesome/`

### Componentes Reutilizáveis
- Todos em `Pages/Components/Geral/`
- 25 componentes (AppContentHeader, AppTableComponent, etc)

### Autenticação
- Toda pasta `Pages/Components/Auth/`
- Toda pasta `Services/Auth/`
- Toda pasta `UseCases/User/`
- UsersController completo
- IdentitySeeder, JWT, Email

### Configurações
- AppSettingsController
- UseCases/AppSettings/
- IndexAppSettings.razor

### Recursos i18n
- Resources/*.resx (todos os idiomas)
- CultureSelector.razor

## Checklist Detalhado

### Remover Controllers
- [ ] ItemsController.cs
- [ ] OrdersController.cs
- [ ] OrdersCartController.cs
- [ ] InvoicesController.cs
- [ ] QuotationsController.cs
- [ ] CreditNotesController.cs
- [ ] DeliveryNotesController.cs
- [ ] IncomingPaymentsController.cs
- [ ] ReturnsController.cs
- [ ] ItemGroupsController.cs
- [ ] ItemExtensionController.cs
- [ ] BusinessPartnersController.cs
- [ ] WarehousesController.cs
- [ ] VatGroupsController.cs
- [ ] SQLQueriesController.cs
- [ ] Attachments2Controller.cs
- [ ] DashboardController.cs (ou adaptar)

### Criar Novos
- [ ] LicensesController.cs
- [ ] RequestLicenseHandler.cs
- [ ] RenewLicenseHandler.cs
- [ ] GetLicensesHandler.cs

### Atualizar Configuração
- [ ] appsettings.json (remover SapB1)
- [ ] Program.cs (remover serviços SAP)
- [ ] NavMenu.razor
- [ ] Sidebar.razor

## Links Relacionados
- [[extranet-sap-estrutura-original]] - Estrutura completa original
- [[licenses-ui-completa]] - UI licenças implementada

**Why:** Mantém o registo claro do plano de reorganização para evitar remover componentes críticos (como o layout AdminLTE ou autenticação) por engano.

**How to apply:** Seguir esta checklist fase por fase. Antes de remover qualquer ficheiro, consultar [[extranet-sap-estrutura-original]] para confirmar que não faz parte do layout ou autenticação a manter.
