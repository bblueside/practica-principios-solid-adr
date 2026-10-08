# ADR-002: Abstraer los canales de notificación detrás de INotificacion

* Estado: Aceptado
* Fecha: 2026-10-03
* Autores: Juliana Velandia

## Contexto

En el Legacy, `NotificacionServicio.EnviarEmailYSms` mezclaba en un solo método el envío por SMTP y la simulación del SMS por Twilio. El Gestor lo instanciaba directamente. Agregar un canal (por ejemplo, WhatsApp) o cambiar de proveedor obligaba a modificar una clase existente, lo que viola OCP y SRP. Además, el Gestor dependía de una clase concreta, lo que viola DIP.

## Decisión

Definiremos la interfaz `INotificacion`, con el método `Notificar(string mensaje)`. Cada canal será una implementación propia: `Email` (SMTP) y `SMS` (Twilio). El Gestor recibirá una `INotificacion` por constructor.

**Rationale:** cada canal cambia por razones distintas, como el proveedor, las credenciales o el formato. Separarlos aísla esos cambios, y un canal nuevo se agrega creando una clase sin modificar el código existente.

## Consecuencias

**Positivas**
* Para agregar un canal basta con implementar `INotificacion`, sin tocar el Gestor (OCP). Esto mejora la extensibilidad.
* Cada canal tiene una única responsabilidad (SRP).

**Negativas**
* **El Gestor ahora notifica por un solo canal.** El Legacy enviaba email y SMS en cada evento; hoy solo se usa la instancia inyectada (`SMS` en `Program`). Para recuperar los dos canales se necesita un compuesto, por ejemplo una `NotificacionMultiple : INotificacion`.

## Cumplimiento (Compliance)

* **Revisión de código en el PR:** el Gestor solo depende de `INotificacion`. Ninguna clase de dominio instancia `Email` o `SMS`.
* **Métricas:** el Ce del Gestor no debe incluir clases concretas de notificación.
