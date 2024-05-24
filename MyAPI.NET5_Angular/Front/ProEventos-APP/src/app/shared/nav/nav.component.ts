import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  isCollapsed = false;
  constructor(private router : Router) { }

  ngOnInit() {
  }

  showMenu() : boolean {
    if (this.router.url === '/user/login' ) {
      return false;
    } else {
      return true;
    }
  }
}
