import { request } from 'graphql-request'
import { Person } from './Models';

export default class GraphClient {
  constructor(private endpoint: string) {
  }
  
  sendRequest<T>(query: string, base: string) {
    return new Promise<T>(resolve => {
      request(this.endpoint, query)
        .then((data: any) => resolve(data[base]))
        .catch(err => {
          console.log(err);

          // TODO: error handling
          //reject(err);
        });
    });
  }

  getPersonId(name: string) {
    return new Promise<number>(resolve => this.sendRequest(`{
      searchPeople(query: "${name}") {
        id
      }
    }`, 'searchPeople').then((data: any) => resolve(parseInt(data[0].id, 10)))
    );
  }

  getPersonWithMovies(personId: number) {
    return this.sendRequest<Person>(`{
      person(id: "${personId}") {
        name
        appearsIn(limit: 100) {
          ... on Movie {
            id
            title
            releaseDate
            backdrop {
              medium
            }
            overview
            popularity
            poster {
              thumbnail
            }
            score
            votes
          }
        }
      }
    }
    `, 'person');
  }
}