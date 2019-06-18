import { Movie, sortMoviesByDate } from "./Models";
import { SearchQuery } from "./SearchQuery";
import {List} from 'immutable';

export interface StoreState {
    allMovies: List<Movie>;
    currentMovies: List<Movie>;
    query: string;
    maxPopularity: number;
    maxVotes: number;
}

const initialState: StoreState = {
    allMovies: List<Movie>(),
    currentMovies: List<Movie>(),
    query: '',
    maxPopularity: 0,
    maxVotes: 0
}

const SET_MOVIES = 'SET_MOVIES';
const SET_QUERY = 'SET_QUERY';

interface SetMoviesAction {
    type: typeof SET_MOVIES;
    movies: Array<Movie>;
}

interface SetQueryAction {
    type: typeof SET_QUERY;
    query: string;
}

type ActionTypes = SetMoviesAction | SetQueryAction;

export function setMovies(movies: Array<Movie>): ActionTypes {
    return {
        type: SET_MOVIES,
        movies
    };
}

export function setQuery(query: string): ActionTypes {
    return {
        type: SET_QUERY,
        query
    };
}

export function moviesReducer(state = initialState, action: ActionTypes) {
    switch(action.type) {
        case SET_MOVIES:
            const validMovies = List<Movie>(action.movies.filter(m => m.title && m.releaseDate));
            let maxScore = 0, maxPopularity = 0, maxVotes = 0;
            validMovies.forEach(m => {
                if(m.score > maxScore)
                    maxScore = m.score;
                if(m.popularity > maxPopularity)
                    maxPopularity = m.popularity;
                if(m.votes > maxVotes)
                    maxVotes = m.votes;
            });
            return {
                ...state,
                allMovies: validMovies,
                currentMovies: sortMoviesByDate(validMovies),
                maxScore,
                maxPopularity,
                maxVotes
            };

        case SET_QUERY:
            const query = new SearchQuery(action.query);
            return {
                ...state,
                query: action.query,
                currentMovies: sortMoviesByDate(query.search(state.allMovies)),
            };
    }
    return state;
}