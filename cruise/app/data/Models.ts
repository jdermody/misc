import { List } from "immutable";

export interface Movie {
    id: string,
    title: string,
    releaseDate: string,
    backdrop: {
      medium: string
    },
    overview: string,
    popularity: number,
    poster: {
      thumbnail: string
    },
    score: number,
    votes: number
}

export interface Person {
    name: string,
    appearsIn: Array<Movie>;
}

function orderByReleaseDate(m1: Movie, m2: Movie) {
    return new Date(m2.releaseDate).valueOf() - new Date(m1.releaseDate).valueOf();
}

export function sortMoviesByDate(movies: List<Movie>) {
    return movies.sort(orderByReleaseDate);
}

export function formatDate(date: Date) {
  var monthNames = [
      "January", "February", "March",
      "April", "May", "June", "July",
      "August", "September", "October",
      "November", "December"
  ];

  var monthIndex = date.getMonth();
  var year = date.getFullYear();

  return monthNames[monthIndex] + ' ' + year;
}