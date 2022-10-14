export interface Vehiculo
{
    idVehiculo: string,
    patente: string,
    cliente: string,
    modelo: string,
    direccion: string,
    telefono: string,
    mail: string,
    nroMotor: string,
    numeroChasis: string,
    cuit: string
}

export enum state
{
    viewing,
    creating,
    updating,
}