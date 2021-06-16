import { Component, OnInit } from '@angular/core';
import { User } from '../user';
import { UserService } from '../user.service';

import { Contact } from '../contact';
import { ContactService } from '../contact.service';


@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  users: User[] = [];
  contacts: Contact[] = [];

  constructor(private userService: UserService,private contactService: ContactService) { }

  ngOnInit(): void {
    this.getUsers();

    this.getContacts();
  }

  getUsers(): void {
    this.userService.getUsers()
      .subscribe(users => this.users = users.slice(1, 5));
  }

  getContacts(): void {
    this.contactService.getContacts()
      .subscribe(contacts => this.contacts = contacts.slice(1, 5));
  }
}
