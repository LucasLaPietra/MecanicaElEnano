export interface Vehiculo
{
    vehiculoId: string,
    patente: string,
    cliente: string,
    modelo: string,
    direccion: string,
    telefono: string,
    mail: string,
    nroMotor: string,
    numeroChasis: string,
    cuit: string
    presupuestos: Presupuesto[]
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

export enum state
{
    viewing,
    creating,
    updating,
}