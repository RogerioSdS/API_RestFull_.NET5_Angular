import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {

  public eventos: any =[
    {
    Tema: 'Angular',
    Local: 'Birigui'
  },
  {
    Tema: '.Net5',
    Local: 'Araçatuba'
  },
  {
    Tema: 'Angular 12 e suas novidades',
    Local: 'São José do rio preto'
  }
  ]
  constructor() { }

  ngOnInit(): void {
  }

}
