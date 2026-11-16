---
name: extranet-sap-estrutura-original
description: Estrutura completa do projeto ExtranetSAP antes da reorganização - módulos SAP B1, layout AdminLTE, componentes reutilizáveis, controllers, casos de uso
metadata:
  type: project
---

# Estrutura Original ExtranetSAP (Antes da Reorganização)

Documentação completa guardada em: `PROJETO_EXTRANET_SAP_ATUAL.md`

## Arquitetura
- **Clean Architecture**: Domain, Application, Persistence, WebAPI, WebFrontEnd
- **Frontend**: Blazor WebAssembly com AdminLTE 3
- **Backend**: ASP.NET Core 8 com CQRS (MediatR)
- **Base de Dados**: SQL Server + EF Core + ASP.NET Identity

## Módulos Principais

### Manter no Novo Projeto ✅
1. **Autenticação completa** (Login JWT, 2FA, Registo, Confirmação Email)
2. **Layout AdminLTE** (MainLayout, NavMenu, todos os componentes Geral/)
3. **Gestão Utilizadores** (UsersController, UseCases/User/)
4. **Configurações** (AppSettings)
5. **Tema Dark/Light**
6. **Internacionalização** (pt-PT, pt-BR, en-US)

### Remover (SAP Business One) ❌
- 13 Controllers SAP (Items, Orders, Invoices, Quotations, etc)
- 15 UseCases SAP
- 7 Pastas páginas frontend SAP
- 10 Serviços frontend SAP
- SAP_Helps/ (SapB1ServiceLayerClient, SapSessionHandler)
- Models: OrderCart, OrderLineCart, ItemExtension

### Expandir (Licenças) ✨
- Frontend: [[licenses-ui-completa]] - UI já implementada
- Backend: Criar LicensesController com 3 endpoints
- UseCases: RequestLicense, RenewLicense, GetLicenses

## Componentes Layout Reutilizáveis (GUARDAR)

Todos em `WebFrontEnd/Pages/Components/Geral/`:
- AppContentHeader, AppTableComponent, AppTableDetails
- AppPageDetailsLayout, AppTabs, LoadingPage
- Toast, ConfirmModal, DatePicker, MultiSelect
- SidebarMenuComponent, CultureSelector
- CartDropdownComponent, BusyButtonComponent

## Integrações Externas

1. **SAP B1 Service Layer** → Remover
2. **LicenseKey API** → Manter e expandir
   - ClientLicenseApiClient.cs
   - GET api/client/licenses/{cardCode}
3. **Leads API** → Manter
   - LeadApiClient.cs
   - POST api/leads

## Segurança (Manter Tudo)
- JWT Bearer Authentication
- Roles: Manager, Administrator, User
- Email confirmação obrigatória
- Password hashing (ASP.NET Identity)
- CORS configurado

## Base de Dados

### Manter
- AspNetUsers, AspNetRoles (Identity)
- AppSetting

### Remover
- OrderCart, OrderLineCart, ItemExtension

### Criar Novo
- LicenseRequest (se necessário)
- LicenseRenewal (se necessário)

## Estatísticas
- 22 Controllers (13 para remover)
- ~80 Use Case Handlers
- ~65 páginas .razor
- ~25 componentes reutilizáveis
- 3 idiomas

## Links Relacionados
- [[licenses-ui-completa]] - UI de licenças já implementada
- [[adminlte-layout]] - Framework visual a manter
- [[auth-flow]] - Fluxo de autenticação completo

**Why:** Esta memória preserva toda a estrutura original do projeto para consulta durante a reorganização. Permite localizar rapidamente qualquer componente, serviço ou lógica que precise ser mantida, adaptada ou removida no novo projeto focado em licenças.

**How to apply:** Consultar esta memória sempre que precisar:
1. Encontrar um componente de layout para reutilizar
2. Verificar como era implementada uma funcionalidade antes
3. Decidir o que manter vs remover na reorganização
4. Localizar código específico (ex: "onde está o SidebarMenuComponent?")
