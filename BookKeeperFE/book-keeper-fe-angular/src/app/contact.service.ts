import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Contact } from './contact';
// import { CONTACTS } from './mock-contacts';
import { MessageService } from './message.service';



@Injectable({
  providedIn: 'root'
})
export class ContactService {



  // URL to web api.
  //private usersUrl = 'api/users';
  //private usersUrl = 'https://localhost:44339/api/users';
  private contactsUrl = 'http://localhost:10789/api/contacts';
  //private usersUrl = 'localhost:10789/api/users';



  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };



  constructor(
    private http: HttpClient,
    private messageService: MessageService
  ) { }



 

  /** GET users from the server. */
  getContacts(): Observable<Contact[]> {
    return this.http.get<Contact[]>(this.contactsUrl)
      .pipe(
        tap(_ => this.log('fetched contacts')),
        catchError(this.handleError<Contact[]>('getContacts', []))
      );
  }



   

  /** GET contact by id. Will 404 if id not found. */
  getContact(id: number): Observable<Contact> {
    const url = `${this.contactsUrl}/${id}`;
    return this.http.get<Contact>(url).pipe(
      tap(_ => this.log(`fetched contact id=${id}`)),
      catchError(this.handleError<Contact>(`getContact id=${id}`))
    );
  }



  /** PUT: update the Contact on the server */
  updateContact(contact: Contact): Observable<any> {
    return this.http.put(this.contactsUrl, contact, this.httpOptions).pipe(
      tap(_ => this.log(`updated contact id=${contact.id}`)),
      catchError(this.handleError<any>('updateContact'))
    );
  }



  /** POST: add a new Contact to the server */
  addContact(contact: Contact): Observable<Contact> {
    return this.http.post<Contact>(this.contactsUrl, contact, this.httpOptions).pipe(
      tap((newContact: Contact) => this.log(`contact c w/ id=${newContact.id}`)),
      catchError(this.handleError<Contact>('addContact'))
    );
  }



  /** DELETE: delete the Contact from the server */
  deleteContact(id: number): Observable<Contact> {
    const url = `${this.contactsUrl}/${id}`;

    return this.http.delete<Contact>(url, this.httpOptions).pipe(
      tap(_ => this.log(`deleted Contact id=${id}`)),
      catchError(this.handleError<Contact>('deleteContact'))
    );
  }



  /** GET users whose name contains search term */
  searchContacts(term: string): Observable<Contact[]> {
    if (!term.trim()) {
      // If not search term, return empty user array.
      return of([]);
    }
    return this.http.get<Contact[]>(`${this.contactsUrl}/?name=${term}`).pipe(
      tap(x => x.length ?
        this.log(`found Contact matching "${term}"`) :
        this.log(`no Contact matching "${term}"`)),
      catchError(this.handleError<Contact[]>('searchContacts', []))
    );
  }



  /** Log a UserService message with the MessageService. */
  private log(message: string) {
    this.messageService.add(`UserService: ${message}`);
  }



  /**
   * Handle Http operation that failed.
   * Let the app continue.
   * @param operation - name of the operation that failed.
   * @param result - optional value to return as the observable result.
   */
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: Send the error to remote logging infrastructure.
      // (Log to console instead.)
      console.error(error);

      // TODO: Better job of transforming error for user consumption.
      this.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);

    };
  }



}
