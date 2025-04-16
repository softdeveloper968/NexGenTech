declare module server {
	interface getAddressesViewModel {
		addressId: number;
		externalId: string;
		addressTypeId: any;
		addressStreetLine1: string;
		addressStreetLine2: string;
		city: string;
		state: string;
		postalCode: string;
		normalized: boolean;
		deliveryPointBarcode: number;
	}
}
