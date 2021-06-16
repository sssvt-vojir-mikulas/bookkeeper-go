import { Component, OnInit } from '@angular/core';
import { Contact } from '../contact';
import { ContactService } from '../contact.service';

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.css']
})
export class ContactsComponent implements OnInit {

  contacts: Contact[] = [];

  constructor(private contactService: ContactService) { }

  ngOnInit(): void {
    this.getContacts();
  }


  getContacts(): void {
    this.contactService.getContacts()
        .subscribe(contacts => this.contacts = contacts);
  }

  
  add(name: string, ico: string,dic: string, bankAccount: string,mobile: string, email: string,www: string, addressStreet: string,addressCity: string, addressZip: string,addressCountry: string): void {
    name = name.trim();
    ico = ico.trim();
    dic = dic.trim();
    bankAccount = bankAccount.trim();
    mobile = mobile.trim();
    email = email.trim();
    www = www.trim();
    addressStreet = addressStreet.trim();
    addressCity = addressCity.trim();
    addressZip = addressZip.trim();
    addressCountry = addressCountry.trim();
     
    if (!name) { return; }
    this.contactService.addContact({ name,ico,dic,bankAccount,mobile,email,www,addressStreet,addressCity,addressZip,addressCountry } as Contact)
      .subscribe(contact => {
        this.contacts.push(contact)
      });
  }

  delete(contact: Contact): void {
    this.contacts = this.contacts.filter(u => u !== contact);
    this.contactService.deleteContact(contact.id).subscribe();
  }
}
