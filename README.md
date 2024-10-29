# Descrição do projeto

Faz web scraping de informações de ações de empresas brasileiras listadas na B3, aplica filtros e ordena seguindo critérios configurados no arquivo de configurações do projeto.

As informações são extraídas do site: https://www.fundamentus.com.br/resultado.php

Permite exportar a lista completa para arquivos nos formatos JSON, CSV e XML e copiar as informações para a área de transferência.

### Pré-requisitos
Requer SDK dotnet 8.0.302 ou superior: [.NET Downloads (Linux, macOS, and Windows)](https://dotnet.microsoft.com/en-us/download/dotnet)


### Dependências externas (pacotes NuGet)

 - Microsoft.Extensions.Configuration.Binder
 - Microsoft.Extensions.Configuration.Json
 - Newtonsoft.Json
 - HtmlAgilityPack
 
*São baixados de forma automática ao efetuar o build*

### Executar o projeto
No prompt de comandos, na pasta raiz do projeto, digitar o comando: `dotnet run`

![](https://github.com/haroldo-rg/stock-analysis/blob/main/images/print.png)
