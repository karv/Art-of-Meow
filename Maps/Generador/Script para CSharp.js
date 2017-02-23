//esta es la única función que vas a llamar desde C#
function obtenerJSON(opciones) {
	opciones = opciones || {};
	var minLadoNivel = opciones.minLadoNivel || 75;
	var maxLadoNivel = opciones.maxLadoNivel || 75;
	var cantidadDeSalas = opciones.cantidadDeSalas || 10;
	var minLadoSala = opciones.minLadoSala || 5;
	var maxLadoSala = opciones.maxLadoSala || 15;
	var maxAnchoPasillo = opciones.maxAnchoPasillo || 1 //mínimo 1, y los pasillos tienen la mitad de probabilidad de ser de ancho 1 que de otro número (por ejemplo, si maxAnchoPasillo === 3, las probabilidades de cada ancho son 1:20%, 2:40$, 3:40%)

	if (maxLadoNivel < minLadoNivel) throw new Error('maxLadoNivel es menor a minLadoNivel');
	if (!cantidadDeSalas) throw new Error('cantidadDeSalas es ' + cantidadDeSalas + '.');
	if (maxLadoNivel < minLadoNivel) throw new Error('maxLadoNivel es menor a minLadoNivel');
	maxAnchoPasillo--;
	var nivel = []
	var widthNivel = enteroAleatorioEntre(minLadoNivel, maxLadoNivel);
	var heightNivel = enteroAleatorioEntre(minLadoNivel, maxLadoNivel);
	//creo un nivel lleno de paredes
	for (var i = 0; i < widthNivel; i++) {
		nivel[i] = '';
		for (var j = 0; j < heightNivel; j++) nivel[i] += 'W';
	}
	//creo las salas (el constructor de las salas sobreescribe las paredes creadas anteriormente)
	var salas = [];
	for (var i = 0; i < cantidadDeSalas; i++) {
		var punto1 = new Punto(enteroAleatorioEntre(0, widthNivel - 1), enteroAleatorioEntre(0, heightNivel - 1));
		var punto2 = new Punto(enteroAleatorioEntreVariosRangos([{
			min: Math.max(0, punto1.x - maxLadoSala),
			max: Math.max(0, punto1.x - minLadoSala)
		}, {
			min: Math.min(widthNivel - 1, punto1.x + minLadoSala),
			max: Math.min(widthNivel - 1, punto1.x + maxLadoSala)
		}]), enteroAleatorioEntreVariosRangos([{
			min: Math.max(0, punto1.y - maxLadoSala),
			max: Math.max(0, punto1.y - minLadoSala)
		}, {
			min: Math.min(heightNivel - 1, punto1.y + minLadoSala),
			max: Math.min(heightNivel - 1, punto1.y + maxLadoSala)
		}]));
		salas.push(new Sala(punto1, punto2, nivel));
	}
	//creo los pasillos que conectan las salas
	for (var i = 1; i < cantidadDeSalas; i++) {
		var direccion = 'ambas';
		var distanciaMinimaEncontrada = Infinity;
		var salaMasCercanaEncontrada;
		for (var j = 0; j < salas.length; j++) {
			var sala = salas[j];
			var resultado = salas[i].distanciaASala(sala);
			if (!isNaN(resultado.horizontal) && isNaN(resultado.vertical) && resultado.horizontal < distanciaMinimaEncontrada) {
				direccion = 'horizontal';
				distanciaMinimaEncontrada = resultado.horizontal;
				salaMasCercanaEncontrada = sala;
			} else if (!isNaN(resultado.vertical) && isNaN(resultado.horizontal) && resultado.vertical < distanciaMinimaEncontrada) {
				direccion = 'vertical';
				distanciaMinimaEncontrada = resultado.vertical;
				salaMasCercanaEncontrada = sala;
			}
			if (j === i - 1) j = cantidadDeSalas - 1; //no chequeo la sala actual ni las siguientes, sino que paso directamente a los pasillos
		}
		if (direccion === 'horizontal') {
			var punto1 = new Punto(salas[i].xMin, enteroAleatorioEntre(Math.max(salas[i].yMin, salaMasCercanaEncontrada.yMin), Math.min(salas[i].yMax, salaMasCercanaEncontrada.yMax)));
			var punto2 = new Punto(salaMasCercanaEncontrada.xMin, acotarEntre(0, enteroAleatorioEntre(punto1.y - maxAnchoPasillo, punto1.y + maxAnchoPasillo), heightNivel - 1));
			var pasillo = new Sala(punto1, punto2, nivel);
			salas.push(pasillo);
		} else if (direccion === 'vertical') {
			var punto1 = new Punto(enteroAleatorioEntre(Math.max(salas[i].xMin, salaMasCercanaEncontrada.xMin), Math.min(salas[i].xMax, salaMasCercanaEncontrada.xMax)), salas[i].yMin);
			var punto2 = new Punto(acotarEntre(0, enteroAleatorioEntre(punto1.x - maxAnchoPasillo, punto1.x + maxAnchoPasillo), widthNivel - 1), salaMasCercanaEncontrada.yMin);
			var pasillo = new Sala(punto1, punto2, nivel);
			salas.push(pasillo);
		} else {
			var sala1;
			var sala2;
			if (enteroAleatorioEntre(0, 1)) {
				sala1 = salas[i];
				sala2 = salas[i === 1 ? 0 : enteroAleatorioEntreVariosRangos([{
					min: 0,
					max: i - 1
				}, {
					min: cantidadDeSalas,
					max: salas.length - 1 //se supone que al ser 1<i, cantidadDeSalas<=salas.length
				}])];
			} else {
				sala1 = salas[i];
				sala2 = salas[i === 1 ? 0 : enteroAleatorioEntreVariosRangos([{
					min: 0,
					max: i - 1
				}, {
					min: cantidadDeSalas,
					max: salas.length - 1 //se supone que al ser 1<i, cantidadDeSalas<=salas.length
				}])];
			}
			var punto1 = new Punto(enteroAleatorioEntre(sala1.xMin, sala1.xMax), enteroAleatorioEntre(sala2.yMin, sala2.yMax));
			var punto2 = new Punto(acotarEntre(0, enteroAleatorioEntre(punto1.x - maxAnchoPasillo, punto1.x + maxAnchoPasillo), widthNivel - 1), acotarEntre(0, enteroAleatorioEntre(punto1.y - maxAnchoPasillo, punto1.y + maxAnchoPasillo), heightNivel - 1));
			var pasillo1 = new Sala(punto1, new Punto(enteroAleatorioEntre(sala2.xMin, sala2.xMax), punto2.y), nivel);
			var pasillo2 = new Sala(new Punto(punto1.x, enteroAleatorioEntre(sala1.yMin, sala1.yMax)), punto2, nivel);
			salas.push(pasillo1);
			salas.push(pasillo2);
		}
	}
	return JSON.stringify(nivel);
}

class Punto {
	constructor(x, y) {
		x = parseInt(x);
		y = parseInt(y);
		if (isFinite(x) && isFinite(y)) {
			this.x = x;
			this.y = y;
		} else throw new Error('Alguna coordenada del punto que estoy intentando crear no es un número.');
	}
}

class Segmento {
	constructor(p, q) {
		if (p instanceof Punto && q instanceof Punto) {
			if (p.x === q.x) {
				this.tipo = 'vertical';
				this.x = p.x;
				this.yMin = Math.min(p.y, q.y);
				this.yMax = Math.max(p.y, q.y);
			} else if (p.y === q.y) {
				this.tipo = 'horizontal';
				this.xMin = Math.min(p.x, q.x);
				this.xMax = Math.max(p.x, q.x);
				this.y = p.y;
			} else throw new Error('El segmento es diagonal.');
		} else throw new Error('No estoy pasándole dos puntos al constructor del segmento.');
	}

	distanciaHorizontalA(segmento) {
		if (segmento instanceof Segmento) {
			if (this.tipo === 'vertical' && segmento.tipo === 'vertical') {
				if (this.yMin <= segmento.yMin && segmento.yMin <= this.yMax || this.yMin <= segmento.yMax && segmento.yMax <= this.yMax || segmento.xMin <= this.yMin && this.yMin <= segmento.xMax) return Math.abs(this.x - segmento.x);
				else return NaN;
			} else throw new Error('Alguno de los segmentos pasados al método distanciaHorizontalA de la clase Segmento no es vertical.');
		} else throw new Error('No estoy pasándole un segmento al método distanciaHorizontalA de la clase Segmento.');
	}

	distanciaVerticalA(segmento) {
		if (segmento instanceof Segmento) {
			if (this.tipo === 'horizontal' && segmento.tipo === 'horizontal') {
				if (this.xMin <= segmento.xMin && segmento.xMin <= this.xMax || this.xMin <= segmento.xMax && segmento.xMax <= this.xMax || segmento.xMin <= this.xMin && this.xMin <= segmento.xMax) return Math.abs(this.y - segmento.y);
				else return NaN;
			} else throw new Error('Alguno de los segmentos pasados al método distanciaVerticalA de la clase Segmento no es horizontal.');
		} else throw new Error('No estoy pasándole un segmento al método distanciaHorizontalA de la clase Segmento.');
	}
}

class Sala {
	constructor(punto1, punto2, nivel) {
		if (punto1 instanceof Punto && punto2 instanceof Punto) {
			var punto3 = new Punto(punto1.x, punto2.y);
			var punto4 = new Punto(punto2.x, punto1.y);
			this.xMin = Math.min(punto1.x, punto2.x);
			this.xMax = Math.max(punto1.x, punto2.x);
			this.yMin = Math.min(punto1.y, punto2.y);
			this.yMax = Math.max(punto1.y, punto2.y);
			this.lados = [
				new Segmento(punto1, punto3),
				new Segmento(punto3, punto2),
				new Segmento(punto2, punto4),
				new Segmento(punto4, punto1)
			]
			for (var x = this.xMin; x <= this.xMax; x++) {
				nivel[x] = nivel[x].substring(0, this.yMin) + ' '.repeat(1 + this.yMax - this.yMin) + nivel[x].substring(this.yMax + 1);
			}
		}
	}

	puntoAleatorio() {
		return new Punto(enteroAleatorioEntre(this.xMin, this.xMax), enteroAleatorioEntre(this.yMin, this.yMax));
	}

	distanciaASala(sala) {
		if (sala instanceof Sala) {
			var distanciaHorizontal = NaN;
			var distanciaVertical = NaN;
			for (var ladoDeThis of this.lados) {
				for (var ladoDeSala of sala.lados) {
					if (ladoDeThis.tipo === 'vertical' && ladoDeSala.tipo === 'vertical') {
						var distancia = ladoDeThis.distanciaHorizontalA(ladoDeSala);
						if (!isNaN(distancia)) {
							if (isNaN(distanciaHorizontal)) distanciaHorizontal = distancia;
							else distanciaHorizontal = Math.min(distancia, distanciaHorizontal);
						}
					} else if (ladoDeThis.tipo === 'horizontal' && ladoDeSala.tipo === 'horizontal') {
						var distancia = ladoDeThis.distanciaVerticalA(ladoDeSala);
						if (!isNaN(distancia)) {
							if (isNaN(distanciaVertical)) distanciaVertical = distancia;
							else distanciaVertical = Math.min(distancia, distanciaVertical);
						}
					}
				}
			}
			return {
				horizontal: distanciaHorizontal,
				vertical: distanciaVertical
			};
		} else throw new Error('No estoy pasándole un segmento al método distanciaHorizontalA de la clase Segmento.');
	}
}

function enteroAleatorioEntre(a, b) {
	a = parseInt(a);
	b = parseInt(b);
	var min = Math.min(a, b);
	var max = Math.max(a, b);
	if (isFinite(min) && isFinite(max)) {
		return min + Math.floor(Math.random() * (max - min + 1));
	} else throw new Error('Alguno de los argumentos pasados a enteroAleatorioEntre no es un número.');
}

function ordenarPorPropMin(a, b) {
	if (!(a instanceof Object) || !isFinite(a.min) || !(b instanceof Object) || !isFinite(b.max)) throw new Error('Algún argumento pasado a la función ordenarPorPropMin no es un objeto cuya propiedad min es un número.');
	return a.min - b.min;
}

function enteroAleatorioEntreVariosRangos(rangos) {
	if (rangos instanceof Array) {
		var numerosTotales = 0;
		for (var rango of rangos) {
			if (rango instanceof Object || isFinite(rango.min) || isFinite(rango.max)) {
				rango.min = parseInt(rango.min);
				rango.max = parseInt(rango.max);
				if (rango.max < rango.min) throw new Error('Alguno de los mínimos pasados a enteroAleatorioEntreVariosRangos es mayor a alguno de los respectivos máximos pasados a la misma función.');
				else {
					rango.numeros = rango.max + 1 - rango.min;
					numerosTotales += rango.numeros;
				}
			} else throw new Error('Algún elemento del array rangos de la función enteroAleatorioEntreVariosRangos no es un objeto cuyas propiedades min y max son números.');
		}
		rangos.sort(ordenarPorPropMin);
		for (i = 0; i < rangos.length - 1; i++) {
			var rango = rangos[i];
			var siguienteRango = rangos[i + 1]
			if (rango.min <= siguienteRango.min && siguienteRango.min <= rango.max) return enteroAleatorioEntreVariosRangos(rangos.slice(0, i).concat([{
				min: rango.min,
				max: Math.max(rango.max, siguienteRango.max)
			}].concat(rangos.slice(i + 2))));
			rango.probabilidad = rango.numeros / numerosTotales;
		}
		var random = Math.random();
		var numeroASumar = 0;
		for (i = 0; i < rangos.length - 1; i++) {
			var rango = rangos[i];
			if (random < rango.probabilidad + numeroASumar) return enteroAleatorioEntre(rango.min, rango.max);
			else numeroASumar += rango.probabilidad;
		}
		return enteroAleatorioEntre(rangos[rangos.length - 1].min, rangos[rangos.length - 1].max);
	} else throw new Error('El argumento de enteroAleatorioEntreVariosRangos no es un array.');
}

function acotarEntre(min, numero, max) {
	if (isFinite(min) && isFinite(numero) && isFinite(max) && min <= max) return Math.max(min, Math.min(numero, max));
	else throw new Error('Algún argumento pasado a la función acotarEntre no es un número, o el min es mayor al max.')
}