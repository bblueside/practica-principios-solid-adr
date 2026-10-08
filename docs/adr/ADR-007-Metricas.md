# ADR-007: Separar las estadísticas de recaudo y cancelaciones en la clase Metricas

* Estado: Aceptado
* Fecha: 2026-10-05
* Autores: Juliana Velandia

## Contexto

En el Legacy, el Gestor acumulaba estado estadístico (`_totalRecaudadoMes` y `_totalCitasCanceladas`) y lo exponía con dos métodos de consulta. Ese estado no tenía relación con agendar o cancelar: era una responsabilidad de reporte. Por eso el Gestor tenía baja cohesión (LCOM96b = 0.50) y más de una razón para cambiar (viola SRP).

## Decisión

Moveremos las estadísticas a la clase `Metricas` que contiene `RegistrarRecaudo(decimal)`, `RegistrarCancelacion()` y sus totales con escritura privada. 

**Rationale:** el reporte cambia por necesidades de gestión, como nuevos indicadores o periodos, y no por cambios en el flujo de citas. Con la separación, el Gestor queda cohesivo y las métricas pueden evolucionar por su cuenta.

## Consecuencias

**Positivas**
* El Gestor queda con LCOM96b = 0.00 y sin estado mutable propio (SRP).
* Los totales solo se modifican a través de los métodos de registro, porque sus setters son privados (encapsulación).

**Negativas**
* El registro deja de ser automático: quien orquesta el flujo debe acordarse de llamar a `RegistrarRecaudo` y `RegistrarCancelacion`.

## Cumplimiento (Compliance)

* El Gestor no puede declarar campos de acumulación ni contadores. Toda estadística nueva va en `Metricas`.
* El LCOM96b de `GestorCitasOdontologicas` se mantiene en 0.00.
