import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {

  public eventos: any ;
  constructor(private http: HttpClient) { }

  /* O ngOnInit é um método do ciclo de vida do componente no Angular que é chamado automaticamente 
  após a inicialização do componente. É usado para realizar inicializações ou configurações 
  necessárias, como buscar dados de serviços, configurar variáveis ou assinar eventos. 
  É recomendado para tarefas que precisam ser executadas apenas uma vez durante o ciclo de vida 
  do componente.*/
  ngOnInit(): void {
    this.getEventos();
  }

  public getEventos(): void {
    this.http.get('https://localhost:5001/api/eventos').subscribe(
      response => this.eventos = response,
      error => console.log(error)
    )
  }
}
