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
    
    // Fazendo uma requisição GET para a URL 'https://localhost:5001/api/eventos'
    // O método subscribe() é chamado automaticamente quando a requisição for concluída
    // Se a requisição for bem sucedida, o método response é chamado com os dados de retorno
    // Se a requisição não for bem sucedida, o método error é chamado com a mensagem de erro
    this.http.get('https://localhost:5001/api/eventos').subscribe(
      response => { // Dados de retorno da requisição
        this.eventos = response; // Atribuindo os dados de retorno à variável 'eventos' do componente
      },
      error => console.log(error) // Imprimindo a mensagem de erro no console
    )
  }
}
