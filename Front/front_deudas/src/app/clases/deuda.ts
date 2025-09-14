import { IsInt, IsString, IsNotEmpty, IsNumber, Min, IsBoolean } from 'class-validator';

export class Deuda {

    id?: number;

    @IsInt({ message: 'Debe selecionar el amigo que presta' })
    @Min(1, { message: 'Debe selecionar el amigo que presta' })
    idUsuarioPresta?: number;

    @IsInt({ message: 'Debe selecionar el amigo que debe' })
    @Min(1, { message:'Debe selecionar el amigo que debe' })
    idUsuarioDebe?: number;

    @IsString()
    @IsNotEmpty({ message: 'Ingrese una descripción de la deuda' })
    descripcion?: string;

    @IsNumber({ maxDecimalPlaces: 2 })
    @Min(0.01, { message: 'El monto debe ser mayor a 0' })
    monto?: number;

    @IsBoolean()
    pagada?: boolean = false;

    @IsBoolean()
    estado?: boolean = true;
}


