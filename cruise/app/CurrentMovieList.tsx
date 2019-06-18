import React from 'react';
import { connect } from 'react-redux';
import { Movie } from './data/Models';
import { StoreState } from './data/Store.';
import { MovieInfo } from './MovieInfo';
import { List } from 'immutable';

interface CurrentMovieListProps {
    movies: List<Movie>;
    query: string;
    maxPopularity: number;
    maxVotes: number;
}

export const CurrentMovieList = connect(
    (state: StoreState) => ({ movies: state.currentMovies, query: state.query, maxPopularity: state.maxPopularity, maxVotes: state.maxVotes })
)((props: CurrentMovieListProps) => {
    const {movies} = props;

    return movies.isEmpty() 
        ? <p className="nothing-found">Nothing found!</p>
        : <ul className="movies">{props.movies.map(m => 
            <li key={m.id}>
                <MovieInfo movie={m} query={props.query} maxPopularity={props.maxPopularity} maxVotes={props.maxVotes} />
            </li>
        )}</ul>;
});