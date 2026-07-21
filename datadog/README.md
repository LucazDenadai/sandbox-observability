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
2. Copie `.env.example` para `.env` e preencha `DD_API_KEY`.
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

- O Dockerfile instala o pacote `datadog-dotnet-apm`, que expõe um CLR Profiler nativo.
- Variáveis de ambiente no `docker-compose.yml` (`CORECLR_ENABLE_PROFILING`, `CORECLR_PROFILER`, etc.) ativam o profiler, que injeta o tracing automaticamente em ASP.NET Core, HttpClient, etc.
- O Datadog Agent recebe os traces via porta `8126` e os logs via Docker log collection (`DD_LOGS_CONFIG_CONTAINER_COLLECT_ALL`).

## Anotações

_(a preencher conforme os estudos avançarem — dashboards criados, alertas configurados, aprendizados)_

## Prós

- _(a preencher)_

## Contras

- _(a preencher)_
