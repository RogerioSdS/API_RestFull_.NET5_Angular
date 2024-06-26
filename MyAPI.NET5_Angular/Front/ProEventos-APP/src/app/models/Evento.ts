import { Lote } from "./Lote";
import { Palestrante } from "./Palestrante";
import { RedeSocial } from "./RedeSocial";

export interface Evento {
    eventoId: number ;
    local: string ;
    dataEvento? : Date ;
    tema: string ;
    qtdPessoas: number ;
    imagemURL: string ;
    telefone: string ;
    email: string ;
    lotes:  Lote[] ;
    redeSocial:  RedeSocial[];
    palestrantesEventos: Palestrante[];
}
