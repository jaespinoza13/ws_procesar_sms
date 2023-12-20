using Application.Common.Interfaces;
using Application.Common.ISO20022.Models;
using Domain.Parameters;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Application.Common.Validations;


public class HeaderValidation : AbstractValidator<Header>
{
    public HeaderValidation(IParametersInMemory parametersInMemory, ISqlInjectionValidationService sqlInjection)
    {
        Include( new ValidaCanalCoopmego( parametersInMemory, sqlInjection ) );
        Include( new ValidaVariablesConvertiblesaEnteros() );
        Include( new ValidaUsuario( sqlInjection ) );
        Include( new ValidaIp() );
    }
}

public class ValidaCanalCoopmego : AbstractValidator<Header>
{
    public ValidaCanalCoopmego(IParametersInMemory parametersInMemory, ISqlInjectionValidationService sqlInjection)
    {
        RuleFor( x => x )
        .Custom( (header, context) =>
        {
            //Valida palabras con amenza
            var sqlInjectionIsValid = sqlInjection.Validate( "str_nemonico_canal", header.str_nemonico_canal, header.str_id_servicio, header, String.Empty );

            if (!sqlInjectionIsValid)
                context.AddFailure( header.str_nemonico_canal + " no permitido" );

            //Valida el nemonico del canal sea válido
            Parametro parametroCanal = parametersInMemory.FindParameter( header.str_nemonico_canal );

            if (String.IsNullOrEmpty( parametroCanal.str_valor_ini ))
                context.AddFailure( "Canal inválido" );

            //Valida el nemonico del canal sea igual que el nombre de la app
            if (parametroCanal.str_descripcion != header.str_app)
                context.AddFailure( header.str_app + " no pertenece al canal " + header.str_nemonico_canal );
        } );
    }
}


public class ValidaVariablesConvertiblesaEnteros : AbstractValidator<Header>
{
    public ValidaVariablesConvertiblesaEnteros()
    {
        RuleFor( x => x )
        .Custom( (header, context) =>
        {
            var isValid = Regex.IsMatch( header.str_ente, @"^[0-9]+$" );
            if (!isValid)
                context.AddFailure( "Ente inválido" );

            var isValidUser = Regex.IsMatch( header.str_id_usuario, @"^[0-9]+$" );
            if (!isValidUser)
                context.AddFailure( "Id de usuario inválido" );

        } );
    }
}

public class ValidaUsuario : AbstractValidator<Header>
{
    public ValidaUsuario(ISqlInjectionValidationService sqlInjection)
    {
        RuleFor( x => x )
        .Custom( (header, context) =>
        {
            //Valida palabras con amenza
            var sqlInjectionIsValid = sqlInjection.Validate( "str_login", header.str_login, header.str_id_servicio, header, String.Empty );

            if (!sqlInjectionIsValid)
                context.AddFailure( header.str_login + " no permitido" );

        } );
    }
}
public class ValidaIp : AbstractValidator<Header>
{
    public ValidaIp()
    {
        RuleFor( x => x )
        .Custom( (header, context) =>
        {
            var isValid = Regex.IsMatch( header.str_ente, @"^((25[0-5]|2[0-4]\d|[01]?\d\d?)\.){3}(25[0-5]|2[0-4]\d|[01]?\d\d?)$" );
            if (!isValid)
                context.AddFailure( " IP inválida" );

        } );
    }
}
