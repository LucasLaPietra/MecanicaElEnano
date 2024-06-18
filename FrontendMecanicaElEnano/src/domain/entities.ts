export interface Vehiculo
{
    vehiculoId: string,
    patente: string,
    cliente: string,
    modelo: string,
    direccion: string,
    telefono: string,
    mail: string,
    numeroChasis: string,
    cuit: string
    presupuestos: Presupuesto[]
    trabajos: Trabajo[]
    ordenTrabajos: OrdenTrabajo[]
}

export interface Turno
{
    turnoId: string,
    fechayHora: Date,
    detalle: string,
    vehiculo: Vehiculo
}

export interface Trabajo
{
    trabajoId: string,
    fecha: Date,
    km: string,
    trabajosRealizados: string,
    trabajosPendientes: string,
    vehiculoId: string
    repuestos: RepuestoTrabajo[]
}

export interface OrdenTrabajo
{
    ordenTrabajoId: string,
    fecha: Date,
    km: string,
    manifiesto: string,
    mecanico: string,
    vehiculoId: string
}

export interface Presupuesto
{
    presupuestoId: string,
    fecha: Date,
    validoHasta: Date,
    km: string,
    trabajoARealizar: string,
    vehiculoId: string
    repuestos: Repuesto[]
}

export interface Repuesto
{
    repuestoId?: string,
    cantidad: number,
    descripcion: string,
    precio: number,
    tipo: number,
}

export interface RepuestoTrabajo
{
    repuestoTrabajoId?: string,
    cantidad: number,
    descripcion: string,
    precio: number,
    tipo: number,
}

export enum state
{
    viewing,
    creating,
    updating,
}
