# DemoBackShopCore

## Descrição:

O **DemoBackShopCore** é um sistema para gerenciar clientes, (produtos e pedidos). Ele oferece funcionalidades para buscar clientes com paginação, visualizar informações de um cliente específico, adicionar novos clientes, atualizar registros de clientes, realizar importação em lote de clientes, deletar clientes e gerenciar endereços associados. Cada endpoint possui validações robustas para garantir a integridade dos dados e evitar erros no sistema.

---

## Funcionalidades

- **Buscar todos os clientes com paginação:** Permite listar clientes de forma organizada, com suporte a paginação.
- **Pegar um cliente:** Recupera informações detalhadas de um cliente específico pelo ID.
- **Adicionar cliente:** Insere novos clientes no sistema, com validação de dados.
- **Atualizar cliente:** Atualiza informações de um cliente existente.
- **Importação em lote:** Suporte para adicionar vários clientes de uma só vez por meio de importação em lote.
- **Deletar cliente:** Remove um cliente do sistema.
- **Gerenciamento de endereços:** Adiciona ou deleta endereços associados aos clientes.
- **Validações:** Impede a entrada de dados incorretos ou inválidos em todos os endpoints.

---

## Tecnologias Utilizadas

- **.NET** com **C#**
- **Entity Framework Core**
- **SQL Server**
- **Docker**
- **Swagger** (para documentação da API)
- **Postman** (para testes manuais da API, opcional)

---

## Pré-requisitos

- **Docker** instalado na máquina
- **Docker Compose** instalado
