import { Lote } from "./Lote";
import { Palestrante } from "./Palestrante";
import { RedeSocial } from "./RedeSocial";

export interface Evento {
    eventoId: number ;
    local: string ;
    dateTime? : Date ;
    tema: string ;
    qtdPessoas: number ;
    imagemURL: string ;
    telefone: string ;
    email: string ;
    lote:  Lote[] ;
    redeSocial:  RedeSocial[];
    palestrantesEventos: Palestrante[];
}
