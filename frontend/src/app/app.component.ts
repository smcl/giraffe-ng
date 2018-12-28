import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

interface User {
  login: string;
  email: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'frontend';

  public users: User[] = [];

  constructor(private http: HttpClient) {
  }

  ngOnInit(): void {
    this.http.get<User[]>('/users').subscribe(users => {
      this.users = users;
    });
  }
}
