import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '@app/services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  isCollapsed = false;
  constructor(public accountService: AccountService,
              private router : Router) { }

  ngOnInit() {
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/user/login');
  }

  showMenu() : boolean {
    if (this.router.url === '/user/login' ) {
      return false;
    } else {
      return true;
    }
  }
}
