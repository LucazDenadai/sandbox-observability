# Datadog

Estudo de APM, logs e infraestrutura usando Datadog, com uma API mínima em .NET rodando em Docker.

## Estrutura

```
datadog/
  src/                  # API .NET (ObservabilityDemo.Api)
  docker-compose.yml    # API + Datadog Agent
  .env.example          # variaveis necessarias (copiar para .env)
```

## Como rodar

1. Crie uma conta trial em https://www.datadoghq.com/ e gere uma API Key em *Organization Settings > API Keys*.
2. Copie `.env.example` para `.env` e preencha `DD_API_KEY` e `DD_SITE` (ex: `us5.datadoghq.com`).
3. Suba os containers:

```bash
docker compose up --build
```

4. A API estará em `http://localhost:8080`. Endpoints disponíveis:
   - `GET /weatherforecast` — endpoint padrão do template, gera log de info
   - `GET /slow` — simula latência aleatória (200ms–1.5s), útil para ver traces lentos
   - `GET /error` — força um erro 500, útil para ver alertas e error tracking

5. No Datadog, acesse **APM > Traces** para ver os traces do serviço `observability-demo-api`, e **Logs** para ver os logs correlacionados aos traces (via `DD_LOGS_INJECTION=true`).

## Como a instrumentação funciona

Usamos **auto-instrumentação** via [.NET Tracer do Datadog](https://docs.datadoghq.com/tracing/trace_collection/dd_libraries/dotnet-core/), sem alterar código da aplicação:

- O Dockerfile copia o tracer nativo (`Datadog.Trace.ClrProfiler.Native.so`) da imagem oficial `datadog/dd-lib-dotnet-init` para dentro da imagem da API.
- Variáveis de ambiente no `docker-compose.yml` (`CORECLR_ENABLE_PROFILING`, `CORECLR_PROFILER`, `CORECLR_PROFILER_PATH`) ativam o profiler do .NET, que injeta o tracing automaticamente em ASP.NET Core, HttpClient, etc.
- O Datadog Agent (`registry.datadoghq.com/agent:7`) recebe os traces via porta `8126` e os logs via Docker log collection (`DD_LOGS_CONFIG_CONTAINER_COLLECT_ALL`).

> Nota: o Datadog também oferece um método de **Docker APM Instrumentation** a nível de host (script `install_script_agent7.sh` com `DD_APM_INSTRUMENTATION_ENABLED=docker`), que auto-instrumenta qualquer container ao subir, sem tocar em Dockerfile. Não usamos aqui porque esse script só roda em Linux nativo — no Windows exigiria uma distro WSL completa (Ubuntu, por exemplo) só para isso.

## Anotações

_(a preencher conforme os estudos avançarem — dashboards criados, alertas configurados, aprendizados)_

## Prós

- _(a preencher)_

## Contras

- _(a preencher)_
