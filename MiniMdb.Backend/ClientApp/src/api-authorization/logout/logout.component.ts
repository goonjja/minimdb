import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from '../authorize.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent implements OnInit {
  constructor(
    private authorizeService: AuthorizeService,
    private activatedRoute: ActivatedRoute,
    private router: Router) { }

  async ngOnInit() {
    this.authorizeService.signOut();
    this.router.navigate(['/']);
  }
}
