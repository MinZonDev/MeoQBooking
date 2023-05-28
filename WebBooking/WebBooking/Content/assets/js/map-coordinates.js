var $ = jQuery.noConflict();

$(document).ready(function($) {

    /* ---------------------------------------------------------------------- */
	/*	Contact Map
	/* ---------------------------------------------------------------------- */
	var contact = {"lat":"25.281219100000000000", "lon":"55.393181799999980000"}; //Change a map coordinate here!

	try {
		$('#map').gmap3({
		    action: 'addMarker',
		    latLng: [contact.lat, contact.lon],
		    map:{
		    	center: [contact.lat, contact.lon],
		    	zoom: 14
		   		},
		    },
		    {action: 'setOptions', args:[{scrollwheel:true}]}
		);
	} catch(err) {

	}

});


	

	
