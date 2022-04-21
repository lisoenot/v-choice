import React, { useEffect, useState } from 'react'
import {
	createStyles,
	makeStyles,
	Box,
	List,
	ListItem,
	Typography,
} from '@material-ui/core'
import Pagination from '@material-ui/lab/Pagination'
import { useHistory } from 'react-router-dom'

import FilmCard from '../../card&tiles/FilmCard/FilmCard'
import AddFilmDialog from '../../crud/AddFilmDialog/AddFilmDialog'
import FilmsFilter from '../../atoms/FilmsFilter/FilmsFilter'
import GenreManager from '../../crud/GenresManager/GenresManager'
import { SortingType } from '../../enums/SortingType'
import { FilteringType } from '../../enums/FilteringType'
import { QueryProps } from '../../enums/QueryProps'
import OnPageCountSwitcher from '../../atoms/OnPageCountSwitcher/OnPageCountSwitcher'

const useStyles = makeStyles((theme) => createStyles({
	filmListItem: {
		display: 'block'
	},
	loading: {
		margin: theme.spacing(1),
		fontSize: '20px'
	},
	tools: {
		display: 'flex',
		justifyContent: 'center',
		alignItems: 'center',
		margin: theme.spacing(0, 2),
	},
	controls: {
		display: 'flex',
		flexDirection: 'column',
		alignItems: 'center',
	},
	control: {
		marginBottom: theme.spacing(1)
	},
	pagination: {
		display: 'flex',
		flexDirection: 'row',
		justifyContent: 'center',
		alignItems: 'center',
	},
	paginationLeftItem: {
		marginRight: theme.spacing(4),
	},
}));

function FilmList(props) {
	const classes = useStyles();
	const history = useHistory();

	const [state, setState] = useState({
		films: [],
		loading: true,
		totalFilms: 0,
		currentPage: props.page,
		onPage: props.count,
		byGenreId: props.genre,
		sortingType: props.sortingType,
		withCommentsOnly: props.withCommentsOnly,
		withRateOnly: props.withRateOnly
	});

	useEffect(() => {
		fetch("https://localhost:5001/api/Film?" +
			`PageNumber=${state.currentPage}` +
			`&OnPageCount=${state.onPage}` +
			`&GenreId=${state.byGenreId}` +
			`&SortBy=${state.sortingType}` +
			`&HasCommentsOnly=${state.withCommentsOnly}` +
			`&HasRateOnly=${state.withRateOnly}`
		)
			.then(response => response.json())
			.then(result => {
				setState({
					...state,
					films: result.items,
					totalFilms: result.totalCount,
					loading: false
				});
			});
	}, [
		state.currentPage,
		state.onPage,
		state.byGenreId,
		state.sortingType,
		state.commonOrder,
		state.withCommentsOnly,
		state.withRateOnly
	])

	const createCatalogURL = (
		p,
		c,
		g = 0,
		sort = SortingType['not-set'],
		withCommentsOnly = false,
		withRateOnly = false
	) => {
		const url = `/catalog/${QueryProps.Page}=${p}&${QueryProps.Count}=${c}`;

		if (g && g !== 0) {
			url += `&${QueryProps.GenreId}=${g}`;
		}

		if (sort && sort !== SortingType['not-set']) {
			url += `&${QueryProps.SortBy}=${sort}`;
		}

		let filter = FilteringType.NotSet;
		if (withCommentsOnly || withRateOnly) {
			if (withCommentsOnly) {
				filter = withRateOnly ? FilteringType.RatedCommented : FilteringType.Commented;
			}
			else {
				filter = FilteringType.Rated;
			}
		}

		if (filter !== FilteringType.NotSet) {
			url += `&${QueryProps.Filter}=${filter}`;
		}

		return url;
	}

	const handleFiltersChanged = (g, s, cf, rf) => {
		history.replace({ pathname: createCatalogURL(1, state.onPage, g, s, cf, rf) })
		setState({
			...state,
			currentPage: 1,
			byGenreId: g,
			sortingType: s,
			withCommentsOnly: cf,
			withRateOnly: rf,
			loading: true
		});
	}

	const handleChangePage = (_, newPage) => {
		history.replace({
			pathname: createCatalogURL(
				newPage,
				state.onPage,
				state.byGenreId,
				state.sortingType,
				state.withCommentsOnly,
				state.withRateOnly
			)
		})
		setState({ ...state, currentPage: newPage, loading: true })
	}

	const handleChangeOnPageCount = (event) => {
		const newCount = event.target.value;
		history.replace({
			pathname: createCatalogURL(
				1,
				newCount,
				state.byGenreId,
				state.sortingType,
				state.withCommentsOnly,
				state.withRateOnly
			)
		});
		setState({ ...state, currentPage: 1, onPage: newCount, loading: true });
	}

	const calculatePagesCount = () => {
		let value = Math.floor(state.totalFilms / state.onPage);
		return value * state.onPage === state.totalFilms ? value : value + 1;
	}

	const handleUpdateFilm = (film) => {
		let arr = [...state.films];
		let found = arr.find(f => f.id === film.id);
		if (found) {
			found = { ...film };
		}
		setState({ ...state, films: [...arr] });
	}

	const handleDeleteFilm = (film) => {
		setState({ ...state, films: state.films.filter(f => f.id !== film.id) });
	}

	return (
		<>
			{
				state.loading
					? <Typography className={classes.loading}>
						Загрузка...
					</Typography >
					: <>
						<Box>
							{
								props.shouldShowControls
									? <Box>
										<Box className={classes.tools}>
											<FilmsFilter
												onSubmit={handleFiltersChanged}
												genres={props.genres}
												selectedGenre={state.byGenreId}
												selectedSortType={state.sortingType}
												selectedCF={state.withCommentsOnly}
												selectedRF={state.withRateOnly}
											/>
										</Box>
										<Box className={classes.tools} >
											<AddFilmDialog genres={props.genres} />
											<GenreManager genres={props.genres}
												onCreate={props.onGenreCreate}
												onUpdate={props.onGenreUpdate}
												onDelete={props.onGenreDelete} />
										</Box>
									</Box>
									: <Box className={classes.tools}>
										<FilmsFilter
											onSubmit={handleFiltersChanged}
											genres={props.genres}
											selectedGenre={state.byGenreId}
											selectedSortType={state.sortingType}
											selectedCF={state.withCommentsOnly}
											selectedRF={state.withRateOnly}
										/>
									</Box>
							}
							<Box>
								<List>
									{
										state.films.length !== 0
											? state.films.map(film => {
												return (
													<ListItem
														className={classes.filmListItem}
														key={film.id}
													>
														<FilmCard
															film={film}
															onUpdate={handleUpdateFilm}
															onDelete={handleDeleteFilm}
															shouldShowControls={props.shouldShowControls}
															genres={props.genres} />
													</ListItem>
												)
											})
											: <Typography variant="h5">
												Не найдено фильмов по запросу
											</Typography>
									}
								</List>
							</Box>
						</Box>
						<Box className={classes.pagination}>
							<Box className={classes.paginationLeftItem}>
								<Pagination
									page={Number(state.currentPage)}
									count={calculatePagesCount()}
									variant="outlined"
									color="primary"
									onChange={handleChangePage}
								/>
							</Box>
							<OnPageCountSwitcher count={state.onPage} onChange={handleChangeOnPageCount} />
						</Box>
					</>
			}
		</>
	)
}

export default FilmList
