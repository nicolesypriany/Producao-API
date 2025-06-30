# **PRODUÇÃO-API** #

API desenvolvida para centralizar informações e facilitar o gerenciamento de processos produtivos na indústria de artefatos de cimento.

[![.NET](https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=fff)](#) [![C#](https://custom-icon-badges.demolab.com/badge/C%23-%23239120.svg?logo=cshrp&logoColor=white)](#) [![Postgres](https://img.shields.io/badge/Postgres-%23316192.svg?logo=postgresql&logoColor=white)](#) [![HTML](https://img.shields.io/badge/HTML-%23E34F26.svg?logo=html5&logoColor=white)](#) [![CSS](https://img.shields.io/badge/CSS-1572B6?logo=css3&logoColor=fff)](#) [![JavaScript](https://img.shields.io/badge/JavaScript-F7DF1E?logo=javascript&logoColor=000)](#)

[Aplicação temporariamente fora do ar]

A documentação completa está disponível no [Swagger](https://producao.pro/api/swagger/index.html)  

Para uma melhor visualização das funcionalidades foi desenvolvida uma aplicação frontend, acessível em [producao.pro](https://producao.pro).

# Resumo das funcionalidades #

## 📦 Produções

A principal entidade da aplicação são as **produções**.  
Para listar as produções, utilize o endpoint:

- `GET /ProcessoProducao`  
- Ou acesse via frontend: [Visualizar Produções](https://producao.pro/production/html/index.html)

Também é possível exportar os dados para os formatos:

- **TXT:** `GET /ProcessoProducao/GerarRelatorioTXT`
- **XLSX:** `GET /ProcessoProducao/GerarRelatorioXLSX`

## 🧾 Logs de Alteração

Usuários com perfis **Administrador** ou **Gerente** podem consultar logs de alterações através do endpoint:

- `POST /Log`

---

## 🛠️ Gerenciamento de Componentes da Produção

A API permite gerenciar os principais itens envolvidos na produção:

- Máquinas `GET /Maquina` 
- Formas `GET /Formas`
- Matérias-primas `GET /MateriaPrima`
- Produtos `GET /Produto`

Todos os itens possuem endpoints para listar todos, buscar por ID, criar, atualizar e inativar.

---

## 💰 Custos

### Custo Médio por Produto

Calcule o custo médio de um produto em um determinado período:

- `POST /Custo/CustoMedioPorProdutoEPeriodo`

### Custo Total Mensal

Consulta que retorna o custo mensal total, total de despesas, total de produções, custo das produções, além de uma lista com os detalhes das produções e despesas:

- `POST /Custo/CustoTotalMensal`

---

## 🚚 Cálculo de Frete

Com base no endereço de origem e destino, preço do combustível, média de consumo e dados da carga, o sistema calcula:

- Distância total
- Número de viagens necessárias
- Valor total do frete

Utilize o endpoint:

- `POST /Frete/Calcular`

---

## 🔐 Autenticação e Usuários

A API utiliza autenticação baseada em **JWT (JSON Web Token)**.

### 📥 Registro e Login

Para autenticar um usuário, utilize os seguintes endpoints:

- `POST /User/Registrar`  
  Cria um novo usuário no sistema.

- `POST /User/Login`  
  Autentica um usuário e retorna o token JWT que deve ser utilizado nas requisições seguintes no cabeçalho `Authorization` com o formato: Bearer {seu token}


### 👥 Perfis de Usuário

Os usuários podem ter um dos seguintes cargos:

- **Consultor**: pode visualizar todas as informações no sistema, mas **não pode criar, editar ou inativar** dados.
- **Gerente**: tem acesso total a todas as funcionalidades do sistema.
- **Administrador**: também possui acesso total, incluindo funcionalidades administrativas e de visualização de logs.

> ⚠️ Os endpoints exigem autenticação. Certifique-se de incluir o token JWT no cabeçalho das requisições.
